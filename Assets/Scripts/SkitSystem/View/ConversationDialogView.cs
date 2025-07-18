using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SkitSystem.View
{
    /// <summary>
    ///     スキットシーンの会話ダイアログを表示するためのビュー。
    /// </summary>
    public class ConversationDialogView : MonoBehaviour
    {
        [SerializeField] private Text _conversationText;
        [SerializeField] private Text _displayNameText;
        [SerializeField] private float _textDisplayDuration = 2f;

        private string _currentConversation;
        private CancellationTokenSource _internalCts;

        public bool IsDisplaying { get; private set; }

        private void Start()
        {
            _internalCts = new CancellationTokenSource();
            IsDisplaying = false;

            // 初期化
            _conversationText.text = string.Empty;
            _displayNameText.text = string.Empty;
        }

        public async UniTask ShowConversation(string talkerName, string conversation, CancellationToken token)
        {
            IsDisplaying = true;
            _internalCts?.Cancel(); // 前の表示があればキャンセル
            _internalCts = CancellationTokenSource.CreateLinkedTokenSource(token);

            try
            {
                conversation = TrimTag(conversation); // タグを除去
                
                _displayNameText.text = talkerName ?? string.Empty;
                _conversationText.text = string.Empty;

                _currentConversation = conversation ?? string.Empty;

                var charaTweenDur = string.IsNullOrEmpty(_currentConversation)
                    ? 0f
                    : _textDisplayDuration / _currentConversation.Length;

                foreach (var chara in _currentConversation)
                {
                    _conversationText.text += chara;
                    await UniTask.Delay(TimeSpan.FromSeconds(charaTweenDur), cancellationToken: _internalCts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // 中断されたらその場で終了（ForceShowTextではない）
            }
            finally
            {
                IsDisplaying = false;
            }
        }

        public void ForceShowText()
        {
            if (string.IsNullOrEmpty(_currentConversation)) return;

            // 現在の表示をすぐ終わらせ、全文表示
            _internalCts?.Cancel(); // 表示中のアニメーションを止める
            _conversationText.text = _currentConversation;
            IsDisplaying = false;
        }
        
        private string TrimTag(string rawText)
        {
            // タグを除去する簡易的な実装
            return System.Text.RegularExpressions.Regex.Replace(rawText, @"<[^>]+>", string.Empty);
        }
    }
}