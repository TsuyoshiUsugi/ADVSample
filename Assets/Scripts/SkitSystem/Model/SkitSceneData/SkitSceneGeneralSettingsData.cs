using System.Collections.Generic;
using System.Text;

namespace SkitSystem.Model.SkitSceneData
{
    public class SkitSceneGeneralSettingsData : SkitSceneDataAbstractBase
    {
        /// <summary>
        /// 各キャラの名前と各感情ごとの画像設定を保持する辞書。
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> CharaImageDictionary = new();
        
        /// <summary>
        /// キャラ名と各言語における名前設定を保持する辞書。
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> CharaNameLanguageMap = new();

        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine("SkitSceneGeneralSettingsData:");
            str.AppendLine("  キャラ画像設定:");
            foreach (var chara in CharaImageDictionary)
            {
                str.AppendLine($"    {chara.Key}:");
                foreach (var emote in chara.Value)
                {
                    str.AppendLine($"      {emote.Key} = {emote.Value}");
                }
            }
            str.AppendLine("  キャラ名言語マップ:");
            foreach (var chara in CharaNameLanguageMap)
            {
                str.AppendLine($"    {chara.Key}:");
                foreach (var language in chara.Value)
                {
                    str.AppendLine($"      {language.Key} = {language.Value}");
                }
            }
            return str.ToString();
        }
    }
}
