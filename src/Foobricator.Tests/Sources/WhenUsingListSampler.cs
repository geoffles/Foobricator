using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Parsing;
using Foobricator.Sources;
using Foobricator.Tests.TestTools;
using Foobricator.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Foobricator.Tests.Sources
{
    [TestClass]
    public class WhenUsingListSampler
    {
        private static readonly IDictionary<string, object> Context = new Dictionary<string, object>
        {
            {"notThis", new StringList(new List<object>{"a", "b", "c"})},
            {"testList",new StringList(new List<object>{"Foo", "Bar", "Baz"})}
        };

        private const string nativeListJson = @"{
                name:""sampleFoo"",
                type:""listSampler"",
                source:{
                    type:""stringList"",
                    values: [""Foo"", ""Bar"", ""Baz""]
                }
            }";

        private const string referenceListJson = @"{
                name:""sampleFoo"",
                type:""listSampler"",
                source:{type:""reference"", refersTo:""testList""}
            }";

        [TestMethod]
        public void ThenTheListMustSampleRandomly()
        {
            using (new InjectObjectFactoryContext(Context))
            using (var rand = new TestDataRandomizer())
            {
                var jToken = JToken.Parse(referenceListJson);
                var listSampler = new ObjectFactory().Create(jToken) as ListSampler;

                Assert.IsNotNull(listSampler, "The listSampler did not get created");

                var item = listSampler.GetItem();
                Assert.AreEqual("RandomInt", rand.LastCalled, "The list sampler should be using the randomizer");
                
            }
        }

        [TestMethod]
        public void ThenTheListMustWorkWithReferences()
        {
            using (new InjectObjectFactoryContext(Context))
            using (var rand = new TestDataRandomizer())
            {
                var jToken = JToken.Parse(referenceListJson);
                var listSampler = new ObjectFactory().Create(jToken) as ListSampler;

                Assert.IsNotNull(listSampler, "The listSampler did not get created");

                var item = listSampler.GetItem();
                Assert.AreEqual("Bar", item, "The expected field was not returned by the sampler ([1])");
            }
        }

        [TestMethod]
        public void ThenTheListMustWorkWithStringLists()
        {
            using (var rand = new TestDataRandomizer())
            {
                var jToken = JToken.Parse(nativeListJson);
                var listSampler = new ObjectFactory().Create(jToken) as ListSampler;

                Assert.IsNotNull(listSampler, "The listSampler did not get created");

                var item = listSampler.GetItem();
                Assert.AreEqual("Bar", item, "The expected field was not returned by the sampler ([1])");
            }
        }
    }
}
