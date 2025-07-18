using System.Linq;
using SkitSystem.Common;
using SkitSystem.Model.SkitSceneData;
using UnityEngine;

namespace SkitSystem.Model.SkitDataTagHandler
{
    public class FlagTagHandler : ISkitTagHandler
    {
        public string HandleTagName => "flag";
        public void Handle(string value)
        {
            // フラグの値を設定する処理
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogWarning("フラグが設定されていません");
                return;
            }

            // ここでは、フラグの値を設定するロジックを実装します。
            SkitSceneDataContainer.Instance.SkitSceneData.TryGetValue(nameof(FlagData), out var flagDataList);
            
            
            var flagData = flagDataList?.OfType<FlagData>().FirstOrDefault();

            
            if (flagData == null)
            {
                Debug.LogError("FlagDataが見つかりません。");
                return;
            }
            
            if (flagData.Flags.ContainsKey(value))
            {
                flagData.SetExclusiveFlag(value); // フラグをtrueに設定
            }
        }
    }
}
