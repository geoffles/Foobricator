using System;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class Switch : IFormattable, IDebugInfoProvider
    {
        public readonly ISource Source;
        public readonly IDictionary<object, object> Map;

        public Switch(DataReference reference, IDictionary<object, object> map) : this(reference.Dereference().First() as ISource, map)
        {
        }

        public Switch(ISource source, IDictionary<object, object> map)
        {
            Source = source;
            Map = map;
        }

        public DebugInfo DebugInfo { get; set; }

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
