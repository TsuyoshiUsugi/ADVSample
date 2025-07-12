using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace  SkitSystem.Common
{
    [CreateAssetMenu(fileName = "SkitDataContainer", menuName = "SkitSystem/SkitDataContainer")]
    public class SkitDataLoaderContainer : ScriptableObject
    {
        [Header("ロードするスキットデータの処理のリスト")]
        [SerializeField] private List<SkitDataLoader> _skitDataLoaders = new List<SkitDataLoader>();
        
        [Header("リモートからデータをロードするかどうか")]
        [SerializeField] private bool _isLoadRemoteData = false;
        public List<SkitDataLoader> SkitDataLoaders => _skitDataLoaders;
        
        public async UniTask LoadSkitDataLoaders(CancellationToken token)
        {
            foreach (var skitDataLoader in _skitDataLoaders)
            {
                await skitDataLoader.LoadSkitDataAsync(token, _isLoadRemoteData);
            }
        }
    }
}
