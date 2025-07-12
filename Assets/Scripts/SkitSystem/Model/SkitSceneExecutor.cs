using System.Threading;
using Cysharp.Threading.Tasks;
using SkitSystem.Common;

namespace SkitSystem.Model
{
    /// <summary>
    ///     会話シーンの処理の流れを書くところ
    ///     会話シーンは以下のフローで行われる。
    ///     1．StarterがSkitDataLoaderContainerに登録されている全Loaderのロード処理を走らせる。
    ///     ロードするとLoaderのConvertedDataに変換後のデータが格納される。以降はこれにアクセスすればいい。
    ///     2．
    /// </summary>
    public class SkitSceneExecutor
    {
        public UniTaskCompletionSource<string> AwaitForInput { get; protected set; }
        
        public async UniTask HandleSkitSceneData(SkitSceneDataContainer skitSceneDataContainer, CancellationToken token)
        {
            if (skitSceneDataContainer.SkitSceneData.TryGetValue(nameof(ConversationGroupData),
                    out var conversationGroupDataList))
            {
                foreach (var skitSceneData in conversationGroupDataList)
                {
                    
                }
            }
        }
    }
}