using System;
using UnityEngine;
using UnityEngine.UI;

namespace SkitSystem.View
{
    public class SkitSceneLogViewer : MonoBehaviour
    {
        [SerializeField] private Button _logButton;
        [SerializeField] private LogPrefab _logPrefab;
        [SerializeField] private GameObject _logViewer;
        [SerializeField] private Transform _messageContainer;

        private void Start()
        {
            // ボタンのクリックイベントにログ表示を追加
            _logButton.onClick.RemoveAllListeners();
            _logButton.onClick.AddListener(() => _logViewer.SetActive(!_logViewer.activeSelf));
        }

        public void SetLog(string talkerName, string conversation)
        {
            if (!_logButton || !_logPrefab)
            {
                Debug.LogError("LogButton or LogPrefab is not assigned.");
                return;
            }
            
            var logInstance = Instantiate(_logPrefab, _messageContainer);
            logInstance.SetLog(talkerName, conversation);
            logInstance.gameObject.SetActive(true);
            
        }
    }
}
