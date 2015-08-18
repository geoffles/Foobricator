using System;
using System.Collections.Generic;
using Foobricator.Sources;

namespace Foobricator.Tools
{
    public class DataReference : IDebugInfoProvider
    {
        #region Global Context
        private static IDictionary<string, object> _contextScope = new Dictionary<string, object>();
        public DebugInfo DebugInfo { get; set; }

        public static IDictionary<string, object> CurrentContext()
        {
            return _contextScope;
        }

        #endregion

        private readonly IDictionary<string, object> _context;
        private readonly string _key;

        public DataReference(IDictionary<string, object> context, string key)
        {
            _context = context;
            _key = key;
        }

        public object Dereference()
        {
            object result;
            if (_context.TryGetValue(_key, out result))
            {
                return result;
            }
            return null;
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {

            object value = Dereference();
                
            if (value == null)
            {
                return string.Empty;
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
