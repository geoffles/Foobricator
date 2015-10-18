using System;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    class RandomDate : ISource, IDebugInfoProvider
    {
        public readonly DateTime BaseDate;
        public readonly int RangeUp;

        public RandomDate(DateTime baseDate, int rangeUp)
        {
            BaseDate = baseDate;
            RangeUp = rangeUp;
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Random date from the base date to the range. From <see cref="Foobricator.Sources.ISource"/>
        /// </summary>
        public object GetItem()
        {
            return BaseDate.AddDays(DataRandomizer.Instance.RandomInt()%RangeUp);
        }
    }
}
