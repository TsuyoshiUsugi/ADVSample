using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace SkitSystem.Model
{
    /// <summary>
    ///     会話シーンの処理の流れを書くところ
    ///     会話シーンは以下のフローで行われる。
    ///     1．StarterがSkitDataLoaderContainerに登録されている全Loaderのロード処理を走らせる。
    ///     ロードするとLoaderのConvertedDataに変換後のデータが格納される。以降はこれにアクセスすればいい。
    ///     2．
    /// </summary>
    public abstract class SkitSceneExecutorBase : IDisposable
    {
        public abstract string HandleSkitContextType { get; }
        public UniTaskCompletionSource<string> AwaitForInput { get; protected set; } = new();

        public void Dispose()
        {
            // TODO マネージリソースをここで解放します
        }

        public abstract UniTask HandleSkitSceneData(SkitSceneDataAbstractBase skitSceneData, CancellationToken token);

        public abstract bool TrtGetNextSkitSceneData(out SkitSceneDataAbstractBase nextSkitContextQueue);
    }

    public class ConversationExecutor : SkitSceneExecutorBase
    {
        private readonly ReactiveProperty<ConversationData> _currentConversationData = new();
        private ConversationGroupData _nextConversationGroupData;
        public override string HandleSkitContextType => nameof(ConversationGroupData);
        public ReadOnlyReactiveProperty<ConversationData> CurrentConversationData => _currentConversationData;

        public override async UniTask HandleSkitSceneData(SkitSceneDataAbstractBase skitSceneData,
            CancellationToken token)
        {
            if (skitSceneData is not ConversationGroupData currentConversationGroup) return;

            foreach (var conversation in currentConversationGroup.ConversationData)
            {
                _currentConversationData.Value = conversation;
                Debug.Log(conversation.ToString());
                await AwaitForInput.Task;
                AwaitForInput = new UniTaskCompletionSource<string>();
                //Todo 会話データのタグを処理する仕組みと次のSkitSceneDataAbstractBaseを設定する処理
            }
        }

        public override bool TrtGetNextSkitSceneData(out SkitSceneDataAbstractBase nextSkitContextQueue)
        {
            // 次の会話データがあるかどうかを確認
            if (_currentConversationData.Value != null)
            {
                nextSkitContextQueue = _nextConversationGroupData;
                return true;
            }

            nextSkitContextQueue = null;
            return false;
        }
    }
}