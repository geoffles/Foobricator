using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Tools;

namespace Foobricator.Output
{
    /// <summary>
    /// Emits a literal value
    /// </summary>
    public class Literal: IOutput, IDebugInfoProvider
    {
        /// <summary>
        /// The value of the literal
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Initialise
        /// </summary>
        public Literal(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Emits <c>Value</c>
        /// </summary>
        /// <param name="writer"></param>
        public void Evaluate(TextWriter writer)
        {
            writer.Write(Value);
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }
    }
}
