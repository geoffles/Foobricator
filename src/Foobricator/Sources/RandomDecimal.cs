using System;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class RandomDecimal : ISource, IFormattable, IDebugInfoProvider
    {

        public RandomDecimal(int lower, int upper)
        {
            _upper = upper;
            _lower = lower;
        }
        
        private int _upper;
        private int _lower;
        private int _decimalPlaces;
        public DebugInfo DebugInfo { get; set; }

        public object GetItem()
        {
            int @base = DataRandomizer.Instance.RandomInt()%(_upper - 1 - _lower) + _lower;
            double fraction = DataRandomizer.Instance.RandomDouble();
            return Convert.ToDecimal(fraction) + @base;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((decimal)GetItem()).ToString(format, formatProvider);
        }
    }
}
