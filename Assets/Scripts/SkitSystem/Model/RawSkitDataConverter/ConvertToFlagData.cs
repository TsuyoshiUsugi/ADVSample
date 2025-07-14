using System;
using System.Collections.Generic;
using SkitSystem.Model.SkitSceneData;

namespace SkitSystem.Model.RawSkitDataConverter
{
    [Serializable]
    public class ConvertToFlagData : IRawSkitDataConverter
    {
        public override string ConvertDataType => "FlagData";

        /// <summary>
        ///     スキットデータを変換するインターフェース。
        /// </summary>
        /// <param name="rawData">生データ</param>
        /// <returns>変換後のデータ</returns>
        public override List<SkitSceneDataAbstractBase> Convert(List<string[]> rawData)
        {
            var flagData = new FlagData();
            foreach (var data in rawData)
            {
                if (data.Length < 2) continue; // データが不完全な場合はスキップ

                var flagName = data[0];
                var flagValue = false;

                flagData.Flags[flagName] = flagValue;
            }

            return new List<SkitSceneDataAbstractBase> { flagData };
        }
    }
}