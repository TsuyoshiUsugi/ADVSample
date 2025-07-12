using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SkitSystem.Common;
using UnityEngine;
using R3;

namespace SkitSystem.Model
{
    public class SkitSceneStarter : MonoBehaviour
    {
        public static SkitSceneStarter Instance { get; private set; }

        [Header("生データをロードする処理がまとまったコンテナ")]
        [SerializeField] private SkitDataLoaderContainer _skitDataLoaderContainer;
        
        [Header("実際のスキットシーンのデータを生成するファクトリのリスト")]
        [SerializeField] private List<ISkitDataFactory> _skitDataFactories;

        public IReadOnlyList<ISkitDataFactory> SkitDataFactories => _skitDataFactories;
        public bool IsInitialized { get; private set; }
        
        # region シーン開始時にスキットデータを初期化する
        private void Awake()
        {
            // シングルトン初期化
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSkitSceneData().Forget();
        }

        private async UniTask InitializeSkitSceneData()
        {
            await _skitDataLoaderContainer.LoadSkitDataLoaders(destroyCancellationToken);

            foreach (var skitDataLoader in _skitDataLoaderContainer.SkitDataLoaders)
            {
                var matchSkitDataFactory = _skitDataFactories.Find(x => x.CreateSkitDataName == skitDataLoader.SkitDataName);
                if (matchSkitDataFactory != null)
                {
                    matchSkitDataFactory.CreateSkitDataFromRawData(skitDataLoader.RawData);
                    Debug.Log($"SkitDataFactory: {matchSkitDataFactory.CreateSkitDataName} のスキットデータを生成しました。");
                }
                else
                {
                    Debug.LogWarning($"SkitDataFactory: {skitDataLoader.SkitDataName} が見つかりませんでした。");
                }
            }
            IsInitialized = true;
        }

        public ISkitDataFactory GetFactory(string skitDataName)
        {
            if (string.IsNullOrEmpty(skitDataName))
            {
                Debug.LogError("SkitDataName is null or empty.");
                return null;
            }

            return _skitDataFactories.Find(factory => factory.CreateSkitDataName == skitDataName);
        }
        # endregion
    }
}
