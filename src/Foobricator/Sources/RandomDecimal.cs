using System;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    /// <summary>
    /// Random Decimal source
    /// </summary>
    public class RandomDecimal : ISource, IFormattable, IDebugInfoProvider
    {
        /// <summary>
        /// Initialise for the given range
        /// </summary>
        public RandomDecimal(int lower, int upper)
        {
            _upper = upper;
            _lower = lower;
        }
        
        private int _upper;
        private int _lower;

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Random decimal value between upper and lower. From <see cref="Foobricator.Sources.ISource"/>
        /// </summary>
        public object GetItem()
        {
            int @base = DataRandomizer.Instance.RandomInt()%(_upper - 1 - _lower) + _lower;
            double fraction = DataRandomizer.Instance.RandomDouble();
            return Convert.ToDecimal(fraction) + @base;
        }

        /// <summary>
        /// Format output as per format string
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((decimal)GetItem()).ToString(format, formatProvider);
        }
    }
}
