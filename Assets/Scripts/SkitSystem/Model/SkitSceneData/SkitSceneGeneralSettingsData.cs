using System.Collections.Generic;

namespace SkitSystem.Model.SkitSceneData
{
    public class SkitSceneGeneralSettingsData : SkitSceneDataAbstractBase
    {
        /// <summary>
        /// 各キャラの名前と各感情ごとの画像設定を保持する辞書。
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> CharaImageDictionary = new();
        public Dictionary<string, List<string>> CharaNameLanguageMap = new();
    }
}
