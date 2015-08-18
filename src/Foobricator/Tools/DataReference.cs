using System;
using System.Collections.Generic;

namespace Foobricator.Tools
{
    public class DataReference : IFormattable, IDebugInfoProvider
    {
        private static readonly IDictionary<string, IList<object>> _contextScope = new Dictionary<string, IList<object>>();
        public DebugInfo DebugInfo { get; set; }

        public static IDictionary<string, IList<object>> CurrentContext()
        {
            return _contextScope;
        }
    

        private readonly IDictionary<string, IList<object>> _context;
        private readonly string _key;

        public DataReference(IDictionary<string, IList<object>> context, string key)
        {
            _context = context;
            _key = key;
        }

        public IList<object> Dereference()
        {
            IList<object> result;
            if (_context.TryGetValue(_key, out result))
            {
                return result;
            }
            return null;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            IList<object> values = Dereference();

            if (values == null)
            {
                return string.Empty;
            }
            if (values.Count != 1)
            {
                return string.Empty;
            }

            object value = values[0];
                
            if (value == null)
            {
                return string.Empty;
            }

            //if (value is ISource)
            //{
            //    value = ((ISource) value).GetItem();
            //}

            return value is IFormattable
                    ? ((IFormattable)value).ToString(format, formatProvider)
                    : value.ToString();
        }
    }
}
