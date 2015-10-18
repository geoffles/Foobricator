using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Foobricator.Tools
{
    /// <summary>
    /// Validate against the schema with detailed schema validation errors.
    /// </summary>
    public class RichValidationProvider: IValidationProvider
    {
        private class RichValidation : IValidation
        {
            /// <summary>
            /// Initialise
            /// </summary>
            public RichValidation(bool isValid, IList<ValidationError> messages)
            {
                IsValid = isValid;
                Messages = messages;
            }

            /// <summary>
            /// The messages
            /// </summary>
            public IList<ValidationError> Messages { get; private set; }

            /// <summary>
            /// Validation result
            /// </summary>
            public bool IsValid { get; private set; }

            /// <summary>
            /// Prints all messages
            /// </summary>
            /// <param name="log"></param>
            public void PrintValidationMessages(ILog log)
            {
                foreach (var message in Messages)
                {
                    PrintValidationMessage(message, log);
                }
            }

            private static void PrintValidationMessage(ValidationError error, ILog log, int tabs = 0)
            {
                string tabString = tabs != 0
                    ? String.Concat(Enumerable.Repeat("\t", tabs))
                    : "";
                

                log.Error("{0}({1},{2}):{3}", tabString, error.LineNumber, error.LinePosition, error.Message);
                error.ChildErrors.ToList().ForEach(p => PrintValidationMessage(p, log, tabs + 1));
            }
        }

        /// <summary>
        /// Perform validation for token
        /// </summary>
        public IValidation Validate(JToken item)
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "Foobricator.Schema.json";
            JSchema schema;
            using (Stream manifestStream = assembly.GetManifestResourceStream(resourceName))
            {
                schema = JSchema.Load(new JsonTextReader(new StreamReader(manifestStream)));
            }

            IList<ValidationError> messages;

            var result = item.IsValid(schema, out messages);

            return new RichValidation(result, messages);
        }
    }
}
