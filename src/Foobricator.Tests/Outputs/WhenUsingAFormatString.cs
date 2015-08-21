using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Output;
using Foobricator.Parsing;
using Foobricator.Sources;
using Foobricator.Tests.TestTools;
using Foobricator.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Foobricator.Tests.Outputs
{
    [TestClass]
    public class WhenUsingAFormatString
    {
        [TestMethod]
        public void ThenPlainFormatStringsFormatStringsMustParse()
        {
            const string json = @"{
                                    type:'formatString',
                                    suppressEndLine:true,
                                    format:'{0}',
                                    source:{type:'reference', refersTo:'source'}
                                }";
            using (new InjectObjectFactoryContext(new Dictionary<string, object>
            {
                {"source", new List<object>{100}}
            }))
            {

                var item = JToken.Parse(json);
                var formatString = (FormatString)new ObjectFactory().Create(item);
                
                Assert.IsInstanceOfType(formatString.Source, typeof(DataReference));
                
                Assert.AreEqual("{0}", formatString.Format);

                var sw = new StringWriter();
                formatString.Evaluate(sw);

                var result = sw.ToString();
                Assert.AreEqual("100", result);
            }
        }

        [TestMethod]
        public void ThenPlainFormatStringsInAnArrayMustParse()
        {
            const string json = @"{
                                    type:'formatString',
                                    suppressEndLine:true,
                                    format:['{0}',
                                        '|', 
                                        '{1}'],
                                    source:{type:'reference', refersTo:'source'}
                                }";
            using (new InjectObjectFactoryContext(new Dictionary<string, object>
            {
                {"source", new List<object>{100, 200}}
            }))
            {

                var item = JToken.Parse(json);
                var formatString = (FormatString)new ObjectFactory().Create(item);

                Assert.IsInstanceOfType(formatString.Source, typeof(DataReference));

                Assert.AreEqual("{0}|{1}", formatString.Format);

                var sw = new StringWriter();
                formatString.Evaluate(sw);

                var result = sw.ToString();
                Assert.AreEqual("100|200", result);
            }
        }

        [TestMethod]
        public void ThenRichFormatStringsMustParse()
        {
            const string json = @"{
                                    type:'formatString',
                                    suppressEndLine:true,
                                    format:[{
                                        value:{
                                            type:'singleValue', 
                                            source:{type:'reference', refersTo:'myNumber'}
                                        }
                                    }, 
                                    '|',
                                    {
                                        value:{
                                            type:'singleValue', 
                                            source:{type:'reference', refersTo:'myString'}
                                        }
                                    }]
                                }";
            using (new InjectObjectFactoryContext(new Dictionary<string, object>
            {
                {"myNumber", 100},
                {"myString", "Foo"}
            }))
            {
                var item = JToken.Parse(json);
                var formatString = (FormatString)new ObjectFactory().Create(item);

                Assert.AreEqual("{0}|{1}", formatString.Format);
                Assert.IsInstanceOfType(formatString.Source, typeof(ISource));
                IList<object> source = (IList<object>)((ISource) formatString.Source).GetItem();

                Assert.AreEqual(2, source.Count);
            
                var sw = new StringWriter();
                formatString.Evaluate(sw);

                var result = sw.ToString();
                Assert.AreEqual("100|Foo", result);
            }
        }

        [TestMethod]
        public void ThenRichFormatStringMustNotIterateOutOfScope()
        {
            var myNumber = new TestSource { Count = 100 };
            const string json = @"{
                                    type:'formatString',
                                    suppressEndLine:true,
                                    scope:'Bar',
                                    format:[{
                                        value:{
                                            type:'singleValue', 
                                            source:{type:'reference', refersTo:'myNumber'}
                                        }
                                    }, 
                                    '|',
                                    {
                                        value:{
                                            type:'singleValue', 
                                            source:{type:'reference', refersTo:'myString'}
                                        }
                                    }]
                                }";
            using (new InjectObjectFactoryContext(new Dictionary<string, object>
            {
                {"myNumber", myNumber},
                {"myString", "Foo"}
            }))
            {

                var item = JToken.Parse(json);
                var formatString = (FormatString)new ObjectFactory().Create(item);

                var times = new Times(new List<IOutput> { formatString }, 2, null, "NotBar");

                var sw = new StringWriter();
                times.Evaluate(sw);

                var result = sw.ToString();
                Assert.AreEqual("100|Foo100|Foo", result);

                Assert.AreEqual(100, myNumber.Count);
            }
        }

        [TestMethod]
        public void ThenRichFormatStringMustApplyEachItemsFormat()
        {
            const string json = @"{
                                    type:'formatString',
                                    suppressEndLine:true,
                                    format:[{
                                        value:{
                                            type:'singleValue', 
                                            source:{type:'reference', refersTo:'myNumber'}
                                        },
                                        format:',-10:##0.00'
                                    }, 
                                    '|',
                                    {
                                        value:{
                                            type:'singleValue', 
                                            source:{type:'reference', refersTo:'myString'}
                                        }
                                    }]
                                }";
            using (new InjectObjectFactoryContext(new Dictionary<string, object>
            {
                {"myNumber", 100},
                {"myString", "Foo"}
            }))
            {

                var item = JToken.Parse(json);
                var formatString = (FormatString)new ObjectFactory().Create(item);

                Assert.AreEqual("{0,-10:##0.00}|{1}", formatString.Format);
                Assert.IsInstanceOfType(formatString.Source, typeof(ISource));
                IList<object> source = (IList<object>)((ISource)formatString.Source).GetItem();

                Assert.AreEqual(2, source.Count);

                var sw = new StringWriter();
                formatString.Evaluate(sw);

                var result = sw.ToString();
                Assert.AreEqual("100.00    |Foo", result);
            }
        }
    }
}
