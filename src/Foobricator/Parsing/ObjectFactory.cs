using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Foobricator.Output;
using Foobricator.RootOutput;
using Foobricator.Sources;
using Foobricator.Tools;
using Newtonsoft.Json.Linq;

namespace Foobricator.Parsing
{
    public class ObjectFactory
    {
        public object Create(JToken item)
        {
            string type = (string)item["type"];
            switch (type)
            {
                case "stringList": return CreateStringList(item);
                case "iterator": return CreateIterator(item);
                case "reference": return CreateDataReference(item);
                case "singleValue": return CreateSingleValue(item);
                case "formatString": return CreateFormatString(item);
                case "tupleValue": return CreateTupleValue(item);
                case "times": return CreateTimes(item);
                case "listSampler": return CreateListSampler(item);
                case "randomDate": return CreateRandomDate(item);
                case "randomInt": return CreateRandomInt(item);
                case "randomDecimal": return CreateRandomDecimal(item);
                case "padLeft": return CreatePadLeft(item);
                case "switch": return CreateSwitch(item);
                case "when": return CreateWhen(item);
                case "conditionalOutput": return CreateConditionalOutput(item);
                case "subString": return CreateSubstring(item);
                case "fileOutput": return CreateFileOutput(item);
                case "clipboardOutput": return CreateClipboardOutput(item);
                case "literal": return CreateLiteral(item);
            }
            Log.Instance.Warn(string.Format("Missing implementation for type:'{0}'", type));
            return null;
        }

        private Literal CreateLiteral(JToken item)
        {
            string value = (string) item["value"];

            var result = new Literal(value);
            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private FileOutput CreateFileOutput(JToken item)
        {
            string filename = (string) item["filename"];
            bool? append = (bool?) item["append"];
            var targets = item["targets"].Children().Select(Create).Cast<IOutput>().ToList();

            var result = new FileOutput(filename, targets, append ?? false);
            
            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private ClipboardOutput CreateClipboardOutput(JToken item)
        {
            var targets = item["targets"].Children().Select(Create).Cast<IOutput>().ToList();

            var result = new ClipboardOutput(targets);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private Substring CreateSubstring(JToken item)
        {
            var source = Create(item["source"]);
            var lengthItem = item["length"];
            int? length = null;
            if (lengthItem != null)
            {
                length = (int) lengthItem;
            }
            int start = (int) item["start"];

            var result = source is DataReference
                ? new Substring((DataReference) source, start, length)
                : new Substring((ISource) source, start, length);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private ConditionalOutput CreateConditionalOutput(JToken item)
        {
            When when = CreateWhen(item["when"]);
            var target = item["target"].Select(Create).OfType<IOutput>();



            var result = new ConditionalOutput(target, when);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private When CreateWhen(JToken item)
        {
            var source = Create(item["source"]);
            var opStr = (string)item["op"];
            var rhsItem = item["rightHandSide"];
            object rightHandSide = null;
            switch (rhsItem.Type)
            {
                case JTokenType.String:
                    rightHandSide = (string) rhsItem;
                    break;
                case JTokenType.Integer:
                    rightHandSide = (int) rhsItem;
                    break;
                case JTokenType.Float:
                    rightHandSide = (decimal) rhsItem;
                    break;
                default : throw new ArgumentOutOfRangeException("Unknown right hand side argument type at " + rhsItem.Path);
            }

            When.Op op = (When.Op) 0;
            if (opStr.IndexOf("Eq", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                op |= When.Op.Eq;
            }
            if (opStr.IndexOf("Gt", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                op |= When.Op.Gt;
            }
            if (opStr.IndexOf("Lt", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                op |= When.Op.Lt;
            }
            if (opStr.IndexOf("Not", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                op |= When.Op.Not;
            }

            When result = source is DataReference
                ? new When((DataReference) source, op, rightHandSide)
                : new When((ISource) source, op, rightHandSide);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private Switch CreateSwitch(JToken item)
        {
            var source = Create(item["source"]);
            var mapToken = item["map"];

            var map = mapToken.Children().Select(p =>
            {
                object key;
                var keyToken = p["key"];
                switch (keyToken.Type)
                {
                    case JTokenType.String : 
                        key = (string)keyToken;
                        break;
                    case JTokenType.Integer:
                        key = (int) keyToken;
                        break;
                    case JTokenType.Float:
                        key = (decimal) keyToken;
                        break;
                    default: throw new ArgumentOutOfRangeException("The type is not support for " + p.Path);
                }

                object value;
                var valueToken = p["value"];
                switch (keyToken.Type)
                {
                    case JTokenType.String:
                        value = (string)valueToken;
                        break;
                    case JTokenType.Integer:
                        value = (int)valueToken;
                        break;
                    case JTokenType.Float:
                        value = (decimal)valueToken;
                        break;
                    default: throw new ArgumentOutOfRangeException("The type is not support for " + p.Path);
                }
                
                var values = new KeyValuePair<object, object>(key, value);
                return values;
            })
            .ToDictionary(p => p.Key, p => p.Value);

            var result = source is DataReference
                ? new Switch((DataReference) source, map)
                : new Switch((ISource) source, map);


            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private PadLeft CreatePadLeft(JToken item)
        {
            var source = Create(item["source"]);
            int length = (int) item["length"];
            char character = ((string) item["character"])[0];

            var result = source is DataReference
                ? new PadLeft((DataReference) source, length, character)
                : new PadLeft((ISource) source, length, character);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private TupleValue CreateTupleValue(JToken item)
        {
            var source = Create(item["source"]);
            int index = (int) item["index"];

            TupleValue result = source is DataReference ?  new TupleValue((DataReference)source, index) : new TupleValue((ISource)source, index);
            
            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private RandomDecimal CreateRandomDecimal(JToken item)
        {
            int upper = (int)item["upper"];
            int lower = (int)item["lower"];

            var result = new RandomDecimal(lower, upper);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private RandomInt CreateRandomInt(JToken item)
        {
            int upper = (int) item["upper"];
            int lower = (int) item["lower"];

            var result = new RandomInt(lower, upper);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private RandomDate CreateRandomDate(JToken item)
        {
            int rangeUp = (int) item["rangeUp"];
            string baseDateString = (string)item["baseDate"];

            DateTime baseDateTime = DateTime.Parse(baseDateString, CultureInfo.InvariantCulture);

            var result = new RandomDate(baseDateTime, rangeUp);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private ListSampler CreateListSampler(JToken item)
        {
            object source = Create(item["source"]);

            ListSampler result = source is DataReference ? new ListSampler((DataReference)source) : new ListSampler((IList<object>)source);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private SingleValue CreateSingleValue(JToken item)
        {
            var source = CreateDataReference(item["source"]);

            SingleValue result = new SingleValue(source);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private DataReference CreateDataReference(JToken item)
        {
            string refersTo = (string) item["refersTo"];
            DataReference result = new DataReference(DataReference.CurrentContext(), refersTo);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private Iterator CreateIterator(JToken item)
        {
            IEnumerable<object> sources = item["sources"].Select(Create);
            string scope = (string)item["scope"];

            Iterator result = new Iterator(sources.ToList(), scope); 

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private StringList CreateStringList(JToken item)
        {
            IEnumerable<string> values = item["values"].Select(p => (string) p);

            var result = new StringList(values.Cast<object>().ToList());
            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private FormatString CreateFormatString(JToken item)
        {
            var formatItem = item["format"];
            string formatString = formatItem.Type == JTokenType.Array
                ? formatItem.Select(p => (string)p).Aggregate(string.Concat)
                : (string)formatItem;
             
            bool? suppressEndLine = (bool?) item["suppressEndLine"];

            DataReference source = CreateDataReference(item["source"]);

            var result = new FormatString(source, formatString, suppressEndLine ?? false);

            result.DebugInfo = GetDebugInfo(item);

            return result;
        }

        private object CreateTimes(JToken item)
        {
            int count = (int) item["count"];
            string separator = (string) item["separator"];
            string scope = (string) item["scope"];


            var targets = item["targets"].Select(Create);
            if (targets.OfType<IRootOutput>().Any())
            {
                RootTimes result = new RootTimes(targets.Cast<IRootOutput>().ToList(), count, separator, scope);

                result.DebugInfo = GetDebugInfo(item);

                return result;
            }
            else
            {
                Times result = new Times(targets.Cast<IOutput>(), count, separator, scope);

                result.DebugInfo = GetDebugInfo(item);

                return result;
            }

            //IEnumerable<IOutput> targets = item["targets"].Select(Create).Cast<IOutput>();

            
        }

        private DebugInfo GetDebugInfo(JToken item)
        {
            return new DebugInfo
            {
                Path = item.Path,
                NodeText = item.ToString()
            };
        }
    }
}
