using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Foobricator.Tools
{
    public interface IValidation
    {
        bool IsValid { get; }
        void PrintValidationMessages(ILog log);
    }
}
