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
            Iterator.Instances.ToList().ForEach(p => p.Dispose());
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
            Iterator.Instances.ToList().ForEach(p => p.Dispose());
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
            Iterator.Instances.ToList().ForEach(p => p.Dispose());
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
            Iterator.Instances.ToList().ForEach(p => p.Dispose());
        }
    }
}
