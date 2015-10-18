using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Foobricator.Tools
{
    /// <summary>
    /// Interface to represent a validation error
    /// </summary>
    public interface IValidation
    {
        /// <summary>
        /// Validation passed
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Get the validation meessage and print it to the log
        /// </summary>
        void PrintValidationMessages(ILog log);
    }
}
