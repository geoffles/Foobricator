using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Sources;
using Foobricator.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Foobricator.Tests.Sources
{
    [TestClass]
    public class WhenUsingAReference
    {
        private static readonly IDictionary<string, object> Context = new Dictionary<string, object>
        {
            {"notThis", new StringList(new List<object>{"a", "b", "c"})},
            {"testList",new StringList(new List<object>{"Foo", "Bar", "Baz"})}
        };

        [TestMethod]
        public void ThenDerefernceMustResolveTheObject()
        {
            DataReference dataReference = new DataReference(Context, "testList");

            var deref = dataReference.Dereference();

            Assert.AreSame(deref, Context["testList"], "The context object was not resolved");

        }
    }
}
