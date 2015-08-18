using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Foobricator.Tests.Sources
{
    [TestClass]
    public class WhenUsingAnIterator
    {
        private class TestSource : ISource
        {
            public int Count = 0;

            public object GetItem()
            {
                return Count++;
            }
        }

        [TestMethod]
        public void ThenNextMustUpdateFromSources()
        {
            var sources = new List<TestSource>
            {
                new TestSource{ Count = 0 }, 
                new TestSource{ Count = 100 }
            };

            using (var iterator = new Iterator(sources.Cast<object>().ToList()))
            {
                //Incremented on next due to constructor
                Assert.AreEqual(1, sources[0].Count);
                Assert.AreEqual(101, sources[1].Count);

                Assert.AreEqual(0, ((IList<object>)iterator.GetItem())[0] );
                Assert.AreEqual(100, ((IList<object>)iterator.GetItem())[1]);

                Iterator.NextAll();

                Assert.AreEqual(2, sources[0].Count);
                Assert.AreEqual(102, sources[1].Count);

                Assert.AreEqual(1, ((IList<object>)iterator.GetItem())[0]);
                Assert.AreEqual(101, ((IList<object>)iterator.GetItem())[1]);
            }
        }

        /// <summary>
        /// Tests that the iterators pull in correct order
        /// </summary>
        [TestMethod]
        public void ThenNextMustUpdateDependentIterators()
        {
            var sources = new List<TestSource>
            {
                new TestSource{ Count = 0 }, 
                new TestSource{ Count = 100 }
            };

            using (var first = new Iterator(sources.Cast<object>().ToList()))
            using (var second = new Iterator(new List<object>{first}))
            {
                Assert.AreEqual(0, ((IList<object>)first.GetItem())[0]);
                Assert.AreEqual(100, ((IList<object>)first.GetItem())[1]);

                Assert.AreEqual(0, ((IList<object>)second.GetItem())[0]);
                Assert.AreEqual(100, ((IList<object>)second.GetItem())[1]);

                Iterator.NextAll();

                Assert.AreEqual(1, ((IList<object>)first.GetItem())[0]);
                Assert.AreEqual(101, ((IList<object>)first.GetItem())[1]);

                Assert.AreEqual(1, ((IList<object>)second.GetItem())[0]);
                Assert.AreEqual(101, ((IList<object>)second.GetItem())[1]);
            }
        }

        [TestMethod]
        public void ThenSourcesMustBeFlattened()
        {
            var sources = new List<object>
            {
                new TestSource{ Count = 0 }, 
                new TestSource{ Count = 100 },
                new StringList(new List<object>{"a", "b"})
            };

            using (var iterator = new Iterator(sources))
            {
                var result = ((IList<object>) iterator.GetItem());
                Assert.AreEqual(0, result[0]);
                Assert.AreEqual(100, result[1]);
                Assert.AreEqual("a", result[2]);
                Assert.AreEqual("b", result[3]);
            }
        }

        [TestMethod]
        public void ThenSequentialIteratorSourcesMustBeFlattened()
        {
            var sources = new List<object>
            {
                new TestSource{ Count = 0 }, 
                new TestSource{ Count = 100 },
                new StringList(new List<object>{"a", "b"})
            };

            using (var first = new Iterator(sources))
            using (var second = new Iterator(new List<object> { first, sources[2] }))
            {
                var result = ((IList<object>)second.GetItem());

                Assert.AreEqual(0, result[0]);
                Assert.AreEqual(100, result[1]);
                Assert.AreEqual("a", result[2]);
                Assert.AreEqual("b", result[3]);
                
                Assert.AreEqual("a", result[4]);
                Assert.AreEqual("b", result[5]);
            }
        }

        [TestMethod]
        public void ThenGetItemMustNotNext()
        {
            var sources = new List<TestSource>
            {
                new TestSource{ Count = 0 }, 
                new TestSource{ Count = 100 }
            };

            using (var iterator = new Iterator(sources.Cast<object>().ToList()))
            {
                //Incremented on next due to constructor
                Assert.AreEqual(1, sources[0].Count);
                Assert.AreEqual(101, sources[1].Count);

                Assert.AreEqual(0, ((IList<object>)iterator.GetItem())[0]);
                Assert.AreEqual(100, ((IList<object>)iterator.GetItem())[1]);

                //Test again to make sure it doesn't pull
                Assert.AreEqual(0, ((IList<object>)iterator.GetItem())[0]);
                Assert.AreEqual(100, ((IList<object>)iterator.GetItem())[1]);
            }
        }

        [TestMethod]
        public void ThenNewInstancesMustRegister()
        {
            Assert.AreEqual(0, Iterator.Instances.Count, "The iterator context is not clean");

            using (var iterator = new Iterator(new List<object> {new TestSource()}))
            {
                Assert.AreEqual(1, Iterator.Instances.Count, "The iterator did not register");
            }
        }

        [TestMethod]
        public void ThenDisposedInstancesMustDeregister()
        {
            Assert.AreEqual(0, Iterator.Instances.Count, "The iterator context is not clean");

            using (var iterator = new Iterator(new List<object> {new TestSource()}))
            {
                Assert.AreEqual(1, Iterator.Instances.Count, "The iterator did not register");
            }

            Assert.AreEqual(0, Iterator.Instances.Count, "The iterator did not deregister");
        }
    }
}
