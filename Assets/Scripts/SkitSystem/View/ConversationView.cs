using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SkitSystem.View
{
    public class ConversationView : MonoBehaviour
    {
        [SerializeField] private Text _conversationText;
        [SerializeField] private Text _displayNameText;
        [SerializeField] private float _textDisplayDuration = 2f;
        
        public async UniTask ShowConversation(string talkerName, string conversation, CancellationToken token)
        {
            try
            {
                if (conversation == null)
                {
                    _conversationText.text = string.Empty;
                    return;
                }

                if (talkerName == null)
                {
                    _displayNameText.text = string.Empty;
                    return;
                }
                _displayNameText.text = talkerName;
                _conversationText.text = string.Empty;
                var charaTweenDur = string.IsNullOrEmpty(conversation) ? 0 : _textDisplayDuration / conversation.Length;

                foreach (var chara in conversation)
                {
                    _conversationText.text += chara;
                    await UniTask.Delay(TimeSpan.FromSeconds(charaTweenDur), cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた場合、全文を即座に表示
                _conversationText.text = conversation;
            }
        }

    }
}
