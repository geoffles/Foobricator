using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Tools;

namespace Foobricator.Tests.TestTools
{
    public class InjectObjectFactoryContext: IDisposable
    {
        private IDictionary<string, object> _current;

        private FieldInfo _field;

        public InjectObjectFactoryContext(IDictionary<string, object> context)
        {
            _current = DataReference.CurrentContext();

            _field = typeof(DataReference).GetField("_contextScope",
                BindingFlags.Static |
                BindingFlags.NonPublic);

            _field.SetValue(null, context);
        }

        public void Dispose()
        {
            _field.SetValue(null, _current);
        }
    }
}
