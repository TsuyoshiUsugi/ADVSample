using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SkitSystem.Common
{
    /// <summary>
    /// リモートとローカルで共通するデータのロード処理の基底クラス
    /// </summary>
    public interface ISkitDataLoader
    {
        public UniTask LoadSkitDataAsync(CancellationToken token, bool isLoadRemoteData = true);
    }

    /// <summary>
    /// リモートのデータをロードする処理の基底クラス。
    /// スプシのアドレスとロード処理をペアでもち、ロード処理でロードしたデータを保持する。
    /// </summary>
    [CreateAssetMenu(fileName = "SkitDataLoader", menuName = "SkitSystem/SkitDataLoader")]
    public class SkitDataLoader : ScriptableObject, ISkitDataLoader
    {
        [Header("保持するスキットデータの名前")]
        public string SkitDataName = "会話データ";
        
        [Header("スプシのアドレス")]
        public string RemoteSpreadSheetDataKey = "";
        
        [Header("ローカルのAddressableのアドレス")]
        public string LocalAddressablePath = "";
        public List<string[]> RawData {get; set;} = new List<string[]>();

        public async UniTask LoadSkitDataAsync(CancellationToken token, bool isLoadRemoteData = true)
        {
            if (!isLoadRemoteData)
            {
                // ローカルのアドレスからデータをロード
                RawData = await CsvLoader.GetLocalSpreadsheetDataAsync(LocalAddressablePath);
            }
            else
            {
                RawData = await CsvLoader.GetRemoteSpreadsheetDataAsync(RemoteSpreadSheetDataKey);
            }
        }
    }
}
