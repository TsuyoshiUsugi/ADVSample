using System.Collections.Generic;
using UnityEngine;

namespace SkitSystem.Model.SkitSceneData
{
    /// <summary>
    /// 現状のフラグを管理するデータクラス。必ず一つのみしかフラグを建てられない。
    /// </summary>
    public class FlagData : SkitSceneDataAbstractBase
    {
        public Dictionary<string, bool> Flags = new();
        
        public string CurrentFlag
        {
            get
            {
                foreach (var kvp in Flags)
                {
                    if (kvp.Value)
                    {
                        return kvp.Key;
                    }
                }
                return null; // どのフラグも立っていない場合はnullを返す
            }
        }
        
        /// <summary>
        /// 単一のフラグのみをたてる。一つがOnになると他は全てOffになる。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetExclusiveFlag(string key, bool value)
        {
            if (Flags.ContainsKey(key))
            {
                // 全てのフラグを false に
                var keys = new List<string>(Flags.Keys); // 一時リストを使わないとコレクション変更時エラーになる可能性がある
                foreach (var k in keys)
                {
                    Flags[k] = false;
                }

                Flags[key] = value;
            }
            else
            {
                Debug.LogError("FlagDataに存在しないキーを設定しようとしました: " + key);
            }
        }
    }
}
