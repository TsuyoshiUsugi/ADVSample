using System;
using UnityEngine;
using UnityEngine.UI;

namespace SkitSystem.View
{
    public enum CharacterPosition
    {
        Left,
        Center,
        Right
    }

    /// <summary>
    ///     名前の通り、会話キャラクターの画像と背景を表示するためのビュー。
    /// </summary>
    public class ConversationCharaImageAndBackgroundView : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _characterImageLeft;
        [SerializeField] private Image _characterImageCenter;
        [SerializeField] private Image _characterImageRight;

        private void Start()
        {
            ResetImages();
        }

        public void ShowBackground(Sprite backgroundSprite)
        {
            if (_backgroundImage != null)
            {
                _backgroundImage.sprite = backgroundSprite;
                _backgroundImage.enabled = backgroundSprite;
            }
        }
        
        public void ResetImages()
        {
            _backgroundImage.sprite = null;
            _backgroundImage.enabled = false;
            _characterImageLeft.sprite = null;
            _characterImageLeft.enabled = false;
            _characterImageCenter.sprite = null;
            _characterImageCenter.enabled = false;
            _characterImageRight.sprite = null;
            _characterImageRight.enabled = false;
        }

        public void ShowCharacter(Sprite characterSprite, string position)
        {
            switch (position)
            {
                case "左":
                    _characterImageLeft.sprite = characterSprite;
                    _characterImageLeft.enabled = characterSprite;
                    break;
                case "真ん中":
                    _characterImageCenter.sprite = characterSprite;
                    _characterImageCenter.enabled = characterSprite;
                    break;
                case "右":
                    _characterImageRight.sprite = characterSprite;
                    _characterImageRight.enabled = characterSprite;
                    break;
            }
        }
    }
}