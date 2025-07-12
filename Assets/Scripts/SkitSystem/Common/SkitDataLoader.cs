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

        [Header("スプシのアドレス")] public string RemoteSpreadSheetDataKey = "";

        [Header("ローカルのAddressableのアドレス")] public string LocalAddressablePath = "";


        public async UniTask LoadSkitDataAsync(CancellationToken token, SkitSceneDataContainer skitSceneDataContainer,
            bool isLoadRemoteData = true)
        {
            if (!isLoadRemoteData)
            {
                var data = await CsvLoader.GetLocalSpreadsheetDataAsync(LocalAddressablePath);
                var convertedData = Converter.Convert(data);
                if (convertedData != null) skitSceneDataContainer.AddSkitSceneData(Converter.ConvertDataType, convertedData);
            }
            else
            {
                var data = await CsvLoader.GetRemoteSpreadsheetDataAsync(RemoteSpreadSheetDataKey);
                var convertedData = Converter.Convert(data);
                if (convertedData != null) skitSceneDataContainer.AddSkitSceneData(Converter.ConvertDataType, convertedData);
            }
        }
    }
}