using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace SkitSystem.Model.SkitSceneExecutor
{
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
