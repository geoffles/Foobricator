using System;
using System.Collections.Generic;
using Foobricator.Sources;

namespace Foobricator.Tools
{
    /// <summary>
    /// A reference to another named source object.
    /// </summary>
    /// <para>
    /// Named sources are loaded into a context dictionary. Objects are resolved
    /// on <c>Deference</c>.
    /// </para>
    public class DataReference : IDebugInfoProvider
    {
        #region Global Context
        private static IDictionary<string, object> _contextScope = new Dictionary<string, object>();

        /// <summary>
        /// Get the current context of named sources.
        /// </summary>        
        public static IDictionary<string, object> CurrentContext()
        {
            return _contextScope;
        }

        #endregion

        private readonly IDictionary<string, object> _context;
        private readonly string _key;

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Initalise against a context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        public DataReference(IDictionary<string, object> context, string key)
        {
            _context = context;
            _key = key;
        }

        /// <summary>
        /// Resolve the named object from the context bound to this reference.
        /// </summary>
        /// <returns></returns>
        public object Dereference()
        {
            object result;
            if (_context.TryGetValue(_key, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Dereferences and tries to get a string value. See Details.
        /// </summary>
        /// <para>
        /// Attempts to get to an IFormattable, otherwise calls ToString. Sources are evaluated.
        /// </para>
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
