using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SkitSystem.View
{
    public class SkitSceneFader : MonoBehaviour
    {
        
        [Header("フェードの設定")]
        [SerializeField] private float _fadeDuration = 1.0f;
        [SerializeField] private Image _fadeImage;
        
        public bool IsFadeImageActive => _fadeImage && _fadeImage.gameObject.activeSelf;
        
        public void ForceShowFade()
        {
            if (_fadeImage is null) return;

            _fadeImage.gameObject.SetActive(true);
            _fadeImage.color = new Color(0, 0, 0, 1); // 完全に黒で表示
        }
        
        public async UniTask FadeInAsync(CancellationToken token)
        {
            if (_fadeImage is null) return;

            _fadeImage.gameObject.SetActive(true);
            _fadeImage.color = new Color(0, 0, 0, 1); // 完全に黒で開始

            // フェードアウト
            while (_fadeImage.color.a > 0)
            {
                var color = _fadeImage.color;
                color.a -= Time.deltaTime / _fadeDuration;
                _fadeImage.color = color;
                await UniTask.Yield();
            }

            _fadeImage.gameObject.SetActive(false);
        }
        
        public async UniTask FadeOutAsync(CancellationToken token)
        {
            if (_fadeImage is null) return;

            _fadeImage.gameObject.SetActive(true);
            _fadeImage.color = new Color(0, 0, 0, 0); // 完全に透明で開始

            // フェードイン
            while (_fadeImage.color.a < 1)
            {
                var color = _fadeImage.color;
                color.a += Time.deltaTime / _fadeDuration;
                _fadeImage.color = color;
                await UniTask.Yield();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration), cancellationToken: token);
        }
    }
}
