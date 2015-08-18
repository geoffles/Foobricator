using System;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    class RandomInt : ISource, IFormattable, IDebugInfoProvider
    {

        public RandomInt(int lower, int upper)
        {
            _upper = upper;
            _lower = lower;
            Next();
        }

        private int _value;
        private readonly int _upper;
        private readonly int _lower;
        public DebugInfo DebugInfo { get; set; }


        public void Next()
        {
            _value = DataRandomizer.Instance.RandomInt() % (_upper - _lower + 1) + _lower;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }

        public object GetItem()
        {
            var current = _value;
            Next();
            return current;
        }
    }
}
