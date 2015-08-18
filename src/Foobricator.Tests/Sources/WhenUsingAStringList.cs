using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Parsing;
using Foobricator.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Foobricator.Tests.Sources
{
    [TestClass]
    public class WhenUsingAStringList
    {
        [TestMethod]
        public void ThenTheStringListMustHaveTheItems()
        {
            const string stringListJson =
            @"{
                name:""listFoo"", 
                type:""stringList"", 
                values:[""Foo"", ""Bar"", ""Baz""]
            }";

            var item = JToken.Parse(stringListJson);

            var stringList = new ObjectFactory().Create(item) as StringList;

            Assert.IsNotNull(stringList, "The JSON create a stringList");
            Assert.AreEqual(3, stringList.Count, "Expected Number of Items");
            
            Assert.AreEqual("Foo", stringList[0]);
            Assert.AreEqual("Bar", stringList[1]);
            Assert.AreEqual("Baz", stringList[2]);
        }
    }
}
