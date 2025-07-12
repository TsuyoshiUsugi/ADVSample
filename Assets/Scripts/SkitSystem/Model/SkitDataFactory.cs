using System.Collections.Generic;

namespace SkitSystem.Model
{
    public interface ISkitDataFactory
    {
        string CreateSkitDataName { get; }
        List<DefaultSkitData> SkitDataList { get; }
        void CreateSkitDataFromRawData(List<string[]> rawData);
    }
    
    public class SkitDataFactory : ISkitDataFactory
    {
        public string CreateSkitDataName { get; private set; } = "会話データ";
        public List<DefaultSkitData> SkitDataList { get; private set; } = new();

        public void CreateSkitDataFromRawData(List<string[]> rawData)
        {
            SkitDataList?.Clear();
            SkitDataList = new List<DefaultSkitData>();
            foreach (var data in rawData)
            {
                if (data.Length < 5) continue; // データが不完全な場合はスキップ

                var id = data[0];
                var flag = data[1];
                var backgroundImageName = data[2];
                var talkerName = data[3];
                var dialogue = data[4];

                var showCharaDataList = new List<DefaultSkitData.ShowCharaData>();
                for (var i = 5; i < data.Length; i += 3)
                    if (i + 2 < data.Length) // チャラデータが3つの要素を持つことを確認
                    {
                        var charaName = data[i];
                        var charaEmote = data[i + 1];
                        var standPos = data[i + 2];
                        showCharaDataList.Add(new DefaultSkitData.ShowCharaData(charaName, charaEmote, standPos));
                    }

                var skitData = new DefaultSkitData(id, flag, backgroundImageName, talkerName, dialogue,
                    showCharaDataList.ToArray());
                SkitDataList.Add(skitData);
            }
        }
    }
}