using System;
using System.Collections.Generic;
using System.Linq;
using SkitSystem.Common;
using SkitSystem.Model.SkitSceneData;
using UnityEngine;

namespace SkitSystem.Model.RawSkitDataConverter
{
    [Serializable]
    public class ConvertToConversationData : IRawSkitDataConverter
    {
        public override string ConvertDataType => nameof(ConversationGroupData);

        public override List<SkitSceneDataAbstractBase> Convert(List<string[]> rawData)
        {
            var convertData = new List<ConversationGroupData>();
            ConversationGroupData currentGroup = null;

            var charaNameMap = new Dictionary<string, Dictionary<string, string>>();
            if (SkitSceneDataContainer.Instance.SkitSceneData.TryGetValue(nameof(SkitSceneGeneralSettingsData),
                    out var generalSettingsDataList))
            {
                charaNameMap = generalSettingsDataList.OfType<SkitSceneGeneralSettingsData>().FirstOrDefault()
                    ?.CharaNameLanguageMap;
            }

            rawData.RemoveAt(0); // ヘッダー行を削除
            foreach (var data in rawData)
            {
                if (data.Length < 5) continue;

                var id = data[0];
                var flag = data[1];
                var backgroundImageName = data[2];
                var talkerName = data[3];
                var dialogueJp = data[4];
                var dialogueEn = data[5];

                // 表示キャラデータを収集
                var showCharaDataList = new List<ShowCharaData>();
                for (var i = 6; i < data.Length; i += 3)
                    if (i + 2 < data.Length)
                    {
                        var charaName = data[i];
                        var charaEmote = data[i + 1];
                        var standPos = data[i + 2];
                        showCharaDataList.Add(new ShowCharaData(charaName, charaEmote, standPos));
                    }

                var dialogue = SkitSceneDataContainer.Instance.UseLanguage == SkitSceneDataContainer.Language.Japanese
                    ? dialogueJp
                    : dialogueEn;

                if (charaNameMap != null && !string.IsNullOrEmpty(talkerName))
                {
                    talkerName = SkitSceneDataContainer.Instance.UseLanguage == SkitSceneDataContainer.Language.Japanese
                        ? talkerName
                        : charaNameMap[talkerName].GetValueOrDefault(nameof(SkitSceneDataContainer.Language.English), talkerName);
                }

                var conversation = new ConversationData(backgroundImageName, talkerName, dialogue,
                    showCharaDataList.ToArray());

                if (!string.IsNullOrEmpty(id))
                {
                    // 新規グループ作成
                    currentGroup = new ConversationGroupData(id, flag);
                    currentGroup.AddConversation(conversation);
                    convertData.Add(currentGroup);
                }
                else if (currentGroup != null)
                {
                    // 既存グループに会話を追加
                    currentGroup.AddConversation(conversation);
                }
                // else: id も空で currentGroup も null → スキップ
            }

            return convertData.Cast<SkitSceneDataAbstractBase>().ToList();
        }
    }
}