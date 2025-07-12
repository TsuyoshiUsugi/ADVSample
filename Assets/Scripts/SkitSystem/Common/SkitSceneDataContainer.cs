using System.Collections.Generic;
using SkitSystem.Model;
using UnityEngine;

namespace SkitSystem.Common
{
    [CreateAssetMenu(fileName = "SkitSceneDataContainer", menuName = "SkitSystem/SkitSceneDataContainer")]
    public class SkitSceneDataContainer : ScriptableObject
    {
        [Header("スキットシーンのデータが初期化されているかどうか")] public bool IsInitialized;

        /// <summary>
        ///     スキットシーンのデータを保持するコンテナ
        /// </summary>
        public Dictionary<string, List<SkitSceneDataAbstractBase>> SkitSceneData { get; private set; }

        public void AddSkitSceneData(string key, List<SkitSceneDataAbstractBase> data)
        {
            SkitSceneData ??= new Dictionary<string, List<SkitSceneDataAbstractBase>>();

            if (!SkitSceneData.TryAdd(key, data))
                // 既に同じキーのデータが存在する場合,リストに追加する
                SkitSceneData[key].AddRange(data);
        }
    }
}