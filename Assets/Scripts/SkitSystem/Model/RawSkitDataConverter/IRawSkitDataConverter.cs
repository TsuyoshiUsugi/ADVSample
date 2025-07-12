using System;
using System.Collections.Generic;

namespace SkitSystem.Model.RawSkitDataConverter
{
    [Serializable]
    public abstract class IRawSkitDataConverter
    {
        public abstract string ConvertDataType { get; }

        /// <summary>
        ///     スキットデータを変換するインターフェース。
        /// </summary>
        /// <param name="rawData">生データ</param>
        /// <returns>変換後のデータ</returns>
        public abstract List<SkitSceneDataAbstractBase> Convert(List<string[]> rawData);
    }
}