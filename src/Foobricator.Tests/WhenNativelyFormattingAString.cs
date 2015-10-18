using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Foobricator.Tests
{
    /// <summary>
    /// Tests when formatting a string
    /// </summary>
    [TestClass]
    public class WhenNativelyFormattingAString
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

        /// <summary>
        /// Sample usage of custom formatting
        /// </summary>
        [TestMethod]
        public void ThenCustomClassesShouldPrintProperly()
        {
            var s = string.Format("{0}|{0,-10:A}|{0:B}", new Formatted());

        }

        
    }
}
