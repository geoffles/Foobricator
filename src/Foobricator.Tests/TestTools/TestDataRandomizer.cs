using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Tools;

namespace Foobricator.Tests.TestTools
{
    public class TestDataRandomizer : IDataRandomizer, IDisposable
    {
        public string LastCalled {get; set; }

        private IDataRandomizer _current;

        private FieldInfo _field;

        public TestDataRandomizer()
        {
            _current = DataRandomizer.Instance;

            _field = typeof(DataRandomizer).GetField("_instance",
                BindingFlags.Static |
                BindingFlags.NonPublic);

            _field.SetValue(null, this);
        }

        public char UpperChar()
        {
            LastCalled = "UpperChar";
            return 'A';
        }

        public char LowerChar()
        {
            LastCalled = "LowerChar";
            return 'a';
        }

        public char DecimalChar()
        {
            LastCalled = "DecimalChar";
            return '1';
        }

        public bool RandomBool()
        {
            LastCalled = "RandomBool";
            return true;
        }

        public int RandomInt()
        {
            LastCalled = "RandomInt";
            return 1;
        }

        public double RandomDouble()
        {
            LastCalled = "RandomDouble";
            return 1.0;
        }

        public void Dispose()
        {
            _field.SetValue(null, _current);
        }
    }
}
