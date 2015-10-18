using System;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    /// <summary>
    /// Source which is a substring of another
    /// </summary>
    public class Substring : ISource, IFormattable, IDebugInfoProvider
    {
        /// <summary>
        /// Start index
        /// </summary>
        public readonly int Start;

        /// <summary>
        /// Length to copy
        /// </summary>
        public readonly int? Length;

        /// <summary>
        /// Source for string to substring
        /// </summary>
        public readonly ISource Source;

        /// <summary>
        /// Initialise against a reference
        /// </summary>
        public Substring(DataReference reference, int start, int? length = null)
            : this(reference.Dereference() as ISource, start, length)
        {

        }

        /// <summary>
        /// Initialise against a source
        /// </summary>
        public Substring(ISource source, int start, int? length = null)
        {
            Source = source;
            Start = start;
            Length = length;
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Returns the substring. Provides warning if 
        /// index/length are out of range for the source and outputs nothing
        /// </summary>
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

        /// <summary>
        /// Returns the substring. Provides warning if 
        /// index/length are out of range for the source and outputs nothing
        /// From <see cref="Foobricator.Sources.ISource"/>
        /// </summary>
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
