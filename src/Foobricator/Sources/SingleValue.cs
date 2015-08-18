using System;
using System.Collections.Generic;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class SingleValue: IFormattable, IDebugInfoProvider
    {
        public readonly DataReference Reference;

        public SingleValue(DataReference reference)
        {
            Reference = reference;
        }

        public DebugInfo DebugInfo { get; set; }

        //public object GetItem()
        //{
        //    var value = Reference.Dereference();

        //    if (value.Count == 1)
        //    {
        //        return value[0];
        //    }

        //    return null;
        //}

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
