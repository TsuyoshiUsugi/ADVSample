using System.Collections.Generic;

namespace SkitSystem.Model
{
    public class DefaultSkitData : SkitDataBase
    {
        public string DataName { get; set; } = "会話データ";
        public string Id { get; }
        public string Flag { get; }
        public string BackgroundImageName { get; }
        public string TalkerName { get; }
        public string Dialogue { get; }
        public ShowCharaData[] ShowCharaDataList { get; }

        public class ShowCharaData
        {
            public string CharaName { get; }
            public string CharaEmote { get; }
            public string StandPos { get; }
            
            public ShowCharaData(string charaName, string charaEmote, string standPos)
            {
                CharaName = charaName;
                CharaEmote = charaEmote;
                StandPos = standPos;
            }
        }
        
        public DefaultSkitData(string id, string flag, string backgroundImageName, string talkerName, string dialogue, ShowCharaData[] showCharaDataList)
        {
            Id = id;
            Flag = flag;
            BackgroundImageName = backgroundImageName;
            TalkerName = talkerName;
            Dialogue = dialogue;
            ShowCharaDataList = showCharaDataList;
        }
    }
}
