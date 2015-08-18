using System;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class Substring : ISource, IFormattable, IDebugInfoProvider
    {
        public readonly int Start;
        public readonly int? Length;
        public readonly ISource Source;

        public Substring(DataReference reference, int start, int? length = null)
            : this(reference.Dereference().First() as ISource, start, length)
        {

        }

        public Substring(ISource source, int start, int? length = null)
        {
            Source = source;
            Start = start;
            Length = length;
        }

        public DebugInfo DebugInfo { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            object value = Source.GetItem();

            string str = value is string
                ? (string) value
                : value.ToString();

            if (Start > (str.Length - 1))
            {
                Log.Instance.Warn("Start index '{0}' is passed the end of the string '{1}' at '{2}'", Start, str, DebugInfo);
                return string.Empty;
            }

            return Length.HasValue
                ? str.Substring(Start, Length.Value)
                : str.Substring(Start);
        }

        public object GetItem()
        {
            object value = Source.GetItem();

            string str = value is string
                ? (string)value
                : value.ToString();

            if (Start > (str.Length - 1))
            {
                Log.Instance.Warn("Start index '{0}' is passed the end of the string '{1}' at {2}", Start, str, DebugInfo);
                return string.Empty;
            }

            return Length.HasValue
                ? str.Substring(Start, Length.Value)
                : str.Substring(Start);
        }
    }
}
