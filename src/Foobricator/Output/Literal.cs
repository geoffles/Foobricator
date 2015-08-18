using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Tools;

namespace Foobricator.Output
{
    public class Literal: IOutput, IDebugInfoProvider
    {
        public readonly string Value;

        public Literal(string value)
        {
            Value = value;
        }

        public void Evaluate(TextWriter writer)
        {
            writer.Write(Value);
        }

        public DebugInfo DebugInfo { get; set; }
    }
}
