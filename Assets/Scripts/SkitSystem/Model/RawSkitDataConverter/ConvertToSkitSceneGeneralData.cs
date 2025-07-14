using System;
using System.Collections.Generic;
using System.Linq;
using SkitSystem.Common;
using SkitSystem.Model.SkitSceneData;
using UnityEngine;

namespace SkitSystem.Model.RawSkitDataConverter
{
    [Serializable]
    public class ConvertToSkitSceneGeneralData : IRawSkitDataConverter
    {
        public override string ConvertDataType => nameof(SkitSceneGeneralSettingsData);

        public override List<SkitSceneDataAbstractBase> Convert(List<string[]> rawData)
        {
            var generalSettingsData = new SkitSceneGeneralSettingsData();
            // 1行目～　各表示キャラの画像設定
            var charaImageDictionary = new Dictionary<string, Dictionary<string, string>>();

            //1行目の2列目から最後までの要素を使って格納する画像のキーを取得
            // 1行目の2列目から最後までの要素をキーリストとして取得
            var keys = rawData[0].Skip(0).ToList();

            var currentIndex = 1;
            while (currentIndex < rawData.Count - 1 && rawData[currentIndex]?.First() != "名前設定")
            {
                // 1行目の2列目から最後までの要素をキーとして、各キャラの感情ごとの画像設定を取得
                var charaName = rawData[currentIndex][0];
                var charaEmoteDictionary = new Dictionary<string, string>();

                for (var i = 1; i < rawData[currentIndex].Length; i++)
                {
                    if (i < keys.Count)
                    {
                        charaEmoteDictionary[keys[i]] = rawData[currentIndex][i];
                    }
                }

                charaImageDictionary[charaName] = charaEmoteDictionary;
                currentIndex++;
            }
            generalSettingsData.CharaImageDictionary = charaImageDictionary;
            // 2行目～　各表示キャラの各言語での名前設定
            var charaNameLanguageMap = new Dictionary<string, Dictionary<string, string>>();
            keys = rawData[currentIndex].Skip(0).ToList();
            
            currentIndex++; // "名前設定"の行をスキップ
            
            while (currentIndex < rawData.Count)
            {
                // 1行目の2列目から最後までの要素をキーとして、各キャラの言語ごとの名前設定を取得
                var charaName = rawData[currentIndex][0];
                var names = new Dictionary<string, string>();

                for (var i = 1; i < rawData[currentIndex].Length; i++)
                {
                    if (i < keys.Count)
                    {
                        names[keys[i]] = rawData[currentIndex][i];
                    }
                    else
                    {
                        Debug.LogWarning($"行 {currentIndex + 1} の列 {i + 1} に対応するキーがありません。");
                    }
                }
                charaNameLanguageMap[charaName] = names;
                currentIndex++;
            }
            
            generalSettingsData.CharaNameLanguageMap = charaNameLanguageMap;
            return new List<SkitSceneDataAbstractBase> { generalSettingsData };
        }
    }
}