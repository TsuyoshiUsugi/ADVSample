using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using SkitSystem.Model.SkitDataTagHandler;
using UnityEngine;

namespace SkitSystem.Model.SkitSceneExecutor
{
    public class ConversationExecutor : SkitSceneExecutorBase
    {
        private ConversationGroupData _nextConversationGroupData;
        private readonly HashSet<ISkitTagHandler> _skitTagHandlers = new HashSet<ISkitTagHandler>();
        private readonly ReactiveProperty<ConversationData> _currentConversationData = new();
        private static readonly System.Text.RegularExpressions.Regex TagRegex =
            new(@"<(?<tag>\w+)(=(?<value>[^>]+))?>", System.Text.RegularExpressions.RegexOptions.Compiled);

        public override string HandleSkitContextType => nameof(ConversationGroupData);
        public ReadOnlyReactiveProperty<ConversationData> CurrentConversationData => _currentConversationData;

        public ConversationExecutor()
        {
            _skitTagHandlers.Add(new SetFlagTagHandler());
        }
        
        public override async UniTask HandleSkitSceneData(SkitSceneDataAbstractBase skitSceneData,
            CancellationToken token)
        {
            if (skitSceneData is not ConversationGroupData currentConversationGroup) return;

            foreach (var conversation in currentConversationGroup.ConversationData)
            {
                //タグの処理
                ProcessTags(conversation.Dialogue);
                
                _currentConversationData.Value = conversation;
                Debug.Log(conversation.ToString());
                await AwaitForInput.Task;
                AwaitForInput = new UniTaskCompletionSource<string>();
                //Todo 次のSkitSceneDataAbstractBaseを設定する処理
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
        
        private void ProcessTags(string dialog)
        {
            var matches = TagRegex.Matches(dialog);
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                var tagName = match.Groups["tag"].Value;    // 例: "flag"
                var tagValue = match.Groups["value"].Success ? match.Groups["value"].Value : null; // 例: "Event01"

                foreach (var handler in _skitTagHandlers.Where(handler => handler.HandleTagName == tagName))
                {
                    handler.Handle(tagValue);
                    break;
                }
            }
        }
    }
}
