using System;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    /// <summary>
    /// Provide a value mapping
    /// </summary>
    public class Switch : IFormattable, IDebugInfoProvider
    {
        /// <summary>
        /// The source item to map against
        /// </summary>
        public readonly ISource Source;
        
        /// <summary>
        /// List of values to map to
        /// </summary>
        public readonly IDictionary<object, object> Map;

        /// <summary>
        /// Initialise against a reference
        /// </summary>
        public Switch(DataReference reference, IDictionary<object, object> map) : this(reference.Dereference() as ISource, map)
        {
        }

        /// <summary>
        /// Initialise against a source
        /// </summary>
        public Switch(ISource source, IDictionary<object, object> map)
        {
            Source = source;
            Map = map;
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Evaluate the source and map it against the available values. Defaults to nothing
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            object key = Source.GetItem();
            object value;

            if (key is IList<object>)
            {
                key = ((IList<object>) key)[0];
            }

            if (Map.TryGetValue(key, out value))
            {
                if (value is IFormattable)
                {
                    return ((IFormattable) value).ToString(format, formatProvider);
                }
                else
                {
                    return value.ToString();
                }
            }

            return string.Empty;
        }
    }
}
