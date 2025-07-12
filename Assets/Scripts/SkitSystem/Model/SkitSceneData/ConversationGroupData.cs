using System.Collections.Generic;
using System.Text;

namespace SkitSystem.Model
{
    /// <summary>
    ///     会話の表示で使われるデータ
    /// </summary>
    public class ConversationGroupData : SkitSceneDataAbstractBase
    {

        public string Id { get; }
        public string Flag { get; }
        public List<ConversationData> ConversationData { get;}
        public ConversationGroupData(string id, string flag)
        {
            Id = id;
            Flag = flag;
            ConversationData = new List<ConversationData>();
        }

        public void AddConversation(ConversationData conversation)
        {
            ConversationData.Add(conversation);
        }
        
        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine($"ConversationGroupData: Id={Id}, フラグ={Flag}");
            foreach (var conversation in ConversationData)
            {
                str.AppendLine($"  背景={conversation.BackgroundImageName}, 話者={conversation.TalkerName}, 会話={conversation.DialogueJP}");
                foreach (var showChara in conversation.ShowCharaDataList)
                {
                    str.AppendLine($"    表示キャラ: 名前={showChara.CharaName}, 感情={showChara.CharaEmote}, 立ち位置={showChara.StandPos}");
                }
            }
            return str.ToString();
        }
    }
    
    public class ShowCharaData
    {
        public ShowCharaData(string charaName, string charaEmote, string standPos)
        {
            CharaName = charaName;
            CharaEmote = charaEmote;
            StandPos = standPos;
        }

        public string CharaName { get; }
        public string CharaEmote { get; }
        public string StandPos { get; }
    }
        
    public class ConversationData
    {
        public string BackgroundImageName { get; }
        public string TalkerName { get; }
        public string DialogueJP { get; }
        public string DialogueEN { get;}
        
        public ShowCharaData[] ShowCharaDataList { get; }

        public ConversationData(string backgroundImageName, string talkerName, string dialogueJp, string dialogueEn, ShowCharaData[] showCharaDataList)
        {
            BackgroundImageName = backgroundImageName;
            TalkerName = talkerName;
            DialogueJP = dialogueJp;
            DialogueEN = dialogueEn;
            ShowCharaDataList = showCharaDataList;
        }
    }
}