using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Sources;
using Foobricator.Tests.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Foobricator.Tests.Sources
{
    [TestClass]
    public class WhenUsingAnInteratorInAScope
    {
        [TestMethod]
        public void ThenNoScopeGivesADefaultScope()
        {
            var source = new TestSource();
            using (new Iterator(new List<object> {source}, null))
            {
                Iterator.NextAll(Iterator.DefaultScope);
            }

            Assert.AreEqual(2, source.Count);
        }

        [TestMethod]
        public void ThenTimesMustOnlyIterateTheSpecifiedScope()
        {
            var fooSource = new TestSource();
            var barSource = new TestSource();

            
            using (new Iterator(new List<object> {fooSource}, "FooScope"))
            using (new Iterator(new List<object> { barSource }, "BarScope"))
            {
                Iterator.NextAll("FooScope");

                Assert.AreEqual(2, fooSource.Count);
                Assert.AreEqual(1, barSource.Count);

                Iterator.NextAll("BarScope");
                
            }

            Assert.AreEqual(2, fooSource.Count);
            Assert.AreEqual(2, barSource.Count);
        }

        [TestMethod]
        public void ThenTimesMustIterateTheSpecifiedDependendScopes()
        {
            var fooSource = new TestSource();
            var barSource = new TestSource();
            var bndySource = new TestSource();


            using (new Iterator(new List<object> { fooSource }, "FooScope"))
            using (new Iterator(new List<object> { barSource }, "BarScope"))
            using (new Iterator(new List<object> { bndySource }, "BndyScope:FooScope:BarScope"))
            {
                Iterator.NextAll("BndyScope");

                Assert.AreEqual(2, bndySource.Count, "bndySource has not advanced on BndyScope NextAll call");
                Assert.AreEqual(1, fooSource.Count, "fooSource has advanced on BndyScope NextAll call");
                Assert.AreEqual(1, barSource.Count, "barSource has advanced on BndyScope NextAll call");


                Iterator.NextAll("FooScope");

                Assert.AreEqual(3, bndySource.Count, "bndySource has not advanced on FooScope NextAll call");
                Assert.AreEqual(2, fooSource.Count, "fooSource has not advanced on FooScope NextAll call");
                Assert.AreEqual(1, barSource.Count, "barSource has advanced on FooScope NextAll call");

                Iterator.NextAll("BarScope");

            }

            Assert.AreEqual(4, bndySource.Count, "bndySource has not advanced on FooScope NextAll call");
            Assert.AreEqual(2, fooSource.Count, "fooSource has not advanced on FooScope NextAll call");
            Assert.AreEqual(2, barSource.Count, "barSource has not advanced on FooScope NextAll call");
        }
    }
}
