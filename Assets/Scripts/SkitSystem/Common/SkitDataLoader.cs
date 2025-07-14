using System.Threading;
using Cysharp.Threading.Tasks;
using SkitSystem.Model.RawSkitDataConverter;
using UnityEngine;

namespace SkitSystem.Common
{
    /// <summary>
    ///     データをロードする処理の基底クラス。
    ///     スプシのアドレスとロード処理をペアでもち、ロード処理でロードしたデータを保持する。
    /// </summary>
    [CreateAssetMenu(fileName = "SkitDataLoader", menuName = "SkitSystem/SkitDataLoader")]
    public class SkitDataLoader : ScriptableObject
    {
        [Header("生データの変換方法")] [SerializeReference] [SubclassSelector]
        public IRawSkitDataConverter Converter;

        [Header("スプシのアドレス")] [SerializeField]
        private string _remoteSpreadSheetDataKey = "";

        [Header("ローカルのAddressableのアドレス")] private string _localAddressablePath = "";


        public async UniTask LoadSkitDataAsync(CancellationToken token, SkitSceneDataContainer skitSceneDataContainer,
            bool isLoadRemoteData = true)
        {
            if (!isLoadRemoteData)
            {
                var data = await CsvLoader.GetLocalSpreadsheetDataAsync(_localAddressablePath);
                var convertedData = Converter.Convert(data);
                if (convertedData != null) skitSceneDataContainer.AddSkitSceneData(Converter.ConvertDataType, convertedData);
            }
            else
            {
                var data = await CsvLoader.GetRemoteSpreadsheetDataAsync(_remoteSpreadSheetDataKey);
                var convertedData = Converter.Convert(data);
                if (convertedData != null) skitSceneDataContainer.AddSkitSceneData(Converter.ConvertDataType, convertedData);
            }
        }
    }
}