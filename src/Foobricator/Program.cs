using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Foobricator.Parsing;
using Foobricator.RootOutput;
using Foobricator.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Foobricator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            args.Where(p => p.StartsWith("/")).ToList().ForEach(p =>
            {
                if (p.StartsWith("/DebugLevelWarning:"))
                {
                    bool debugInfo = p.EndsWith("true", StringComparison.InvariantCultureIgnoreCase);
                    Log.Instance.DebugInfoOnWarn = debugInfo;
                    Log.Instance.Info("DebugLevelWarning detected. Set to {0}", debugInfo);
                }
            });

            args.Where(p => !p.StartsWith("/")).ToList().ForEach(ProcessInputFile);
            

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static void PrintValidationMessage(ValidationError error, int tabs = 0)
        {
            string tabString = tabs != 0
                ? String.Concat(Enumerable.Repeat("\t", tabs))
                : "";

            Console.WriteLine(tabString + error.Message);
            error.ChildErrors.ToList().ForEach(p => PrintValidationMessage(p, tabs+1));
        }

        private static void ProcessInputFile(string filename)
        {
            Log.Instance.Info("Processing File {0}", filename);

            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "Foobricator.Schema.json";
            Stream manifestStream = assembly.GetManifestResourceStream(resourceName);
            JSchema schema = JSchema.Load(new JsonTextReader(new StreamReader(manifestStream)));
            var inputStream = new StreamReader(filename);
            JsonTextReader reader = new JsonTextReader(inputStream);
            
            IList<ValidationError> messages;
            
            var obj = JToken.ReadFrom(reader);
            var result = obj.IsValid(schema, out messages);


            Console.WriteLine("Valid: {0}", result);

            if (!result)
            {
                messages.ToList().ForEach(p => PrintValidationMessage(p));
                return;
            }

            var contextToken = obj["context"];
            var context = DataReference.CurrentContext();
            ObjectFactory objectFactory = new ObjectFactory();

            foreach (var item in contextToken.Children())
            {
                string type = (string)item["type"];
                string name = (string)item.TryGet("name");
                
                Console.WriteLine("{0}:{1}", name ?? "<unnamed>", type);

                if (name != null)
                {
                    var contextItem = objectFactory.Create(item);
                    if (contextItem != null)
                    {
                        context[name] = contextItem;
                    }
                }
            }
            
            var ouputToken = obj["output"];

            IList<IRootOutput> outputs = new List<IRootOutput>();
            foreach (var item in ouputToken)
            {
                string type = (string)item["type"];

                Console.WriteLine("{0}:{1}", "output", type);

                var outputItem = (IRootOutput)objectFactory.Create(item);

               outputs.Add(outputItem);
            }

            var toExecute = outputs.ToList();

            if (!toExecute.Any())
            {
                Log.Instance.Warn("No root outputs found. No output will be generated. Choose one of (fileOutput)");
            }
            else
            {
                toExecute.ForEach(p => p.Evaluate());
            }

            inputStream.Dispose();
            manifestStream.Dispose();
        }
    }

    static class JsonExtensions
    {
        public static JToken TryGet(this JToken token, string key)
        {
            try
            {
                return token[key];
            }
            catch
            {
                return null;
            }
        }
    }
}
