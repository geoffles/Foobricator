using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Foobricator.Tests
{
    [TestClass]
    public class WhenUsingStringFormat
    {
        private class Formatted : IFormattable
        {

            public override string ToString()
            {
                return "Foo foo";
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                if (format == "A")
                {
                    return "FOO FOO";
                }
                return "Foo foo";
            }
        }

        [TestMethod]
        public void ThenCustomClassesMustAcceptFormattingArguments()
        {
            var s = string.Format("{0}|{0,-10:A}|{0:B}", new Formatted());

            Assert.AreEqual("Foo foo|FOO FOO   |Foo foo", s);

        }
    }
}
