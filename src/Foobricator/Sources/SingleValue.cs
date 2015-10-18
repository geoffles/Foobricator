using System;
using System.Collections.Generic;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    /// <summary>
    /// Evaluate a single value (as opposed to a list, also see <see cref="Foobricator.Sources.TupleValue"/>)
    /// </summary>    
    public class SingleValue: IFormattable, IDebugInfoProvider
    {
        /// <summary>
        /// The reference for the source
        /// </summary>
        public readonly DataReference Reference;

        /// <summary>
        /// Initialise
        /// </summary>
        public SingleValue(DataReference reference)
        {
            Reference = reference;
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Get a single value and attempt to format it
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            object value = Reference.Dereference();

            if (value == null)
            {
                return string.Empty;
            }

            if (value is Iterator)
            {
                var items = (IList<object>)((Iterator) value).GetItem();

                if (items.Count != 1)
                {
                    return string.Empty;
                }

                value = items[0];
            }

            if (value is ISource)
            {
                value = ((ISource)value).GetItem();
            }

            return value is IFormattable
                    ? ((IFormattable)value).ToString(format, formatProvider)
                    : value.ToString();
        }
    }
}
