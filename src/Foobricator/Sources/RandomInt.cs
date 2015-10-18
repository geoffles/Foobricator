using System;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    /// <summary>
    /// Random integer source
    /// </summary>
    public class RandomInt : ISource, IFormattable, IDebugInfoProvider
    {
        /// <summary>
        /// Initalise for the given range
        /// </summary>
        public RandomInt(int lower, int upper)
        {
            _upper = upper;
            _lower = lower;
            Next();
        }

        private int _value;
        private readonly int _upper;
        private readonly int _lower;

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Evaluate the next random int
        /// </summary>
        public void Next()
        {
            _value = DataRandomizer.Instance.RandomInt() % (_upper - _lower + 1) + _lower;
        }

        /// <summary>
        /// Format output as per format string
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }

        /// <summary>
        /// Random integer value. From <see cref="Foobricator.Sources.ISource"/>
        /// </summary>
        public object GetItem()
        {
            var current = _value;
            Next();
            return current;
        }
    }
}
