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
        [SerializeField] private SkitSceneDataContainer _skitSceneDataContainer;

        # region シーン開始時にスキットデータを初期化する

# if UNITY_EDITOR
        private void Start()
        {
            EditorApplication.playModeStateChanged += _ => _skitSceneDataContainer.IsInitialized = false;
        }
#endif

        public async UniTask InitializeSkitSceneData()
        {
            if (_skitSceneDataContainer.IsInitialized)
            {
                return;
            }

            foreach (var skitDataLoader in _skitDataLoaders)
                await skitDataLoader.LoadSkitDataAsync(destroyCancellationToken, _skitSceneDataContainer,
                    _loadRemoteData);
            _skitSceneDataContainer.IsInitialized = true;
        }

        # endregion
    }
}