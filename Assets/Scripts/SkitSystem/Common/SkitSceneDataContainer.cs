using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SkitSystem.Model;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SkitSystem.Common
{
    [CreateAssetMenu(fileName = "SkitSceneDataContainer", menuName = "SkitSystem/SkitSceneDataContainer")]
    public class SkitSceneDataContainer : ScriptableObject
    {
        public enum Language
        {
            Japanese,
            English,
            Chinese,
            Korean
        }

        [Header("スキットシーンのデータが初期化されているかどうか")] public bool IsInitialized;
        [Header("使用する言語")] public Language UseLanguage;

        private AsyncOperationHandle<IList<Sprite>> _handle;
        public Dictionary<string, Sprite> SpriteDictionary { get; private set; }

        /// <summary>
        ///     スキットシーンのデータを保持するコンテナ
        /// </summary>
        public Dictionary<string, List<SkitSceneDataAbstractBase>> SkitSceneData { get; private set; }

        public Dictionary<string, Sprite> SkitSceneImage { get; private set; }

        public void AddSkitSceneData(string key, List<SkitSceneDataAbstractBase> data)
        {
            SkitSceneData ??= new Dictionary<string, List<SkitSceneDataAbstractBase>>();

            if (!SkitSceneData.TryAdd(key, data))
                // 既に同じキーのデータが存在する場合,リストに追加する
                SkitSceneData[key].AddRange(data);
        }

        /// <summary>
        ///     SkitScene ラベルのついた Sprite をすべてロードし、名前でアクセス可能にする
        /// </summary>
        public async UniTask LoadSkitSceneAssetsAsync()
        {
            _handle = Addressables.LoadAssetsAsync<Sprite>("SkitScene");
            var sprites = await _handle.Task;

            // 名前でアクセスしやすいように Dictionary に変換
            SpriteDictionary = sprites.ToDictionary(sprite => sprite.name, sprite => sprite);
        }

        /// <summary>
        ///     名前指定で Sprite を取得
        /// </summary>
        public Sprite GetSprite(string spriteName)
        {
            return SpriteDictionary.GetValueOrDefault(spriteName);
        }

        /// <summary>
        ///     解放処理
        /// </summary>
        public void Unload()
        {
            if (_handle.IsValid()) Addressables.Release(_handle);
            SpriteDictionary.Clear();

            SkitSceneData?.Clear();
        }
    }
}