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

        public DebugInfo DebugInfo { get; set; }

        public object GetItem()
        {
            return BaseDate.AddDays(DataRandomizer.Instance.RandomInt()%RangeUp);
        }
    }
}
