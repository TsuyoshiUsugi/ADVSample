using UnityEngine;
using UnityEngine.UI;

namespace SkitSystem.View
{
    public class LogPrefab : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _messageText;

        public void SetLog(string talkerName, string conversation)
        {
            if (_nameText)
            {
                _nameText.text = talkerName ?? string.Empty;
            }

            if (_messageText)
            {
                _messageText.text = conversation ?? string.Empty;
            }
        }
    }
}
