using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Output;
using Foobricator.Sources;
using Foobricator.Tests.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Foobricator.Tests.Sources
{
    [TestClass]
    public class WhenUsingTimes
    {
        private class TestOutput : IOutput
        {
            public void Evaluate(TextWriter writer)
            {
                if (writer != null)
                {
                    writer.Write("Evaluated");
                }
            }
        }

        [TestMethod]
        public void ThenTimesMustIterateIterators()
        {
            var testSource = new TestSource();
            
            using (new Iterator(new List<object>{ testSource}, null))
            {
                var outputs = new List<IOutput> { new TestOutput() };
                var times = new Times(outputs, 1, null, null);

                var stringWriter = new StringWriter();
                times.Evaluate(stringWriter);

                Assert.AreEqual("Evaluated", stringWriter.ToString());
                Assert.AreEqual(2, testSource.Count);
            }
        }

        [TestMethod]
        public void ThenTimesMustIterateOnlyTheGivenScope()
        {
            var fooSource = new TestSource();
            var barSource = new TestSource();
            var noScopeSource = new TestSource();

            using (new Iterator(new List<object> { fooSource }, "FooScope"))
            using (new Iterator(new List<object> { barSource }, "BarScope"))
            using (new Iterator(new List<object> { noScopeSource }, null))
            {
                var outputs = new List<IOutput> { new TestOutput() };
                var fooTimes = new Times(outputs, 1, null, "FooScope");
                var barTimes = new Times(outputs, 1, null, "BarScope");
                var noScopeTimes = new Times(outputs, 1, null, null);

                
                fooTimes.Evaluate(null);
                Assert.AreEqual(2, fooSource.Count, "FooScope did not advance on foo times");
                Assert.AreEqual(1, barSource.Count, "BarScope advance on foo");
                Assert.AreEqual(1, noScopeSource.Count, "BarScope advance on foo");

                barTimes.Evaluate(null);
                Assert.AreEqual(2, fooSource.Count, "FooScope advance on bar");
                Assert.AreEqual(2, barSource.Count, "BarScope did not advance on bar times");
                Assert.AreEqual(1, noScopeSource.Count, "NoScope advance on bar");

                noScopeTimes.Evaluate(null);
                Assert.AreEqual(2, fooSource.Count, "FooScope advance on noScope");
                Assert.AreEqual(2, barSource.Count, "BarScope advance on noScope");
                Assert.AreEqual(2, noScopeSource.Count, "NoScope did not advance on noScope times");
            }
        }
    }
}
