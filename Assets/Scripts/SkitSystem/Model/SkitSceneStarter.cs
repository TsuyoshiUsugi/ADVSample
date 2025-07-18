using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SkitSystem.Common;
using UnityEditor;
using UnityEngine;

namespace SkitSystem.Model
{
    public class SkitSceneStarter : MonoBehaviour
    {
        [SerializeField] private bool _loadRemoteData = true;
        [SerializeField] private List<SkitDataLoader> _skitDataLoaders = new();
        [SerializeField] private string _skitSceneImageAddressablePath = "SkitScene";

        # region シーン開始時にスキットデータを初期化する

# if UNITY_EDITOR
        private void Start()
        {
            EditorApplication.playModeStateChanged += _ => SkitSceneDataContainer.IsInitialized = false;
        }
#endif

        public async UniTask InitializeSkitSceneData()
        {
            if (SkitSceneDataContainer.IsInitialized) return;
            await UniTask.WaitUntil(() => SkitSceneDataContainer.IsLoaded, cancellationToken: destroyCancellationToken);
            
            // スキットシーンデータのロード
            foreach (var skitDataLoader in _skitDataLoaders)
                await skitDataLoader.LoadSkitDataAsync(destroyCancellationToken, SkitSceneDataContainer.Instance,
                    _loadRemoteData);

            // スキットシーンのスプライトをロード
            await SkitSceneDataContainer.Instance.LoadSkitSceneAssetsAsync();

            SkitSceneDataContainer.IsInitialized = true;
        }

        # endregion
    }
}