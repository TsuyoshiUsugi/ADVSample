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

        public void ShowBackground(Sprite backgroundSprite)
        {
            if (_backgroundImage != null)
            {
                _backgroundImage.sprite = backgroundSprite;
                _backgroundImage.enabled = backgroundSprite;
            }
        }

        public void ShowCharacter(Sprite characterSprite, CharacterPosition position)
        {
            switch (position)
            {
                case CharacterPosition.Left:
                    _characterImageLeft.sprite = characterSprite;
                    _characterImageLeft.enabled = characterSprite;
                    break;
                case CharacterPosition.Center:
                    _characterImageCenter.sprite = characterSprite;
                    _characterImageCenter.enabled = characterSprite;
                    break;
                case CharacterPosition.Right:
                    _characterImageRight.sprite = characterSprite;
                    _characterImageRight.enabled = characterSprite;
                    break;
            }
        }
    }
}