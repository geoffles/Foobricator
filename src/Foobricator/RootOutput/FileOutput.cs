using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foobricator.Output;
using Foobricator.Tools;

namespace Foobricator.RootOutput
{
    /// <summary>
    /// Ouput to a file
    /// </summary>
    public class FileOutput : IRootOutput, IDebugInfoProvider
    {
        /// <summary>
        /// Append to overwrite
        /// </summary>
        public readonly bool Append;
        private bool _mustAppend = false;

        /// <summary>
        /// The filename
        /// </summary>
        public readonly string Filename;

        /// <summary>
        /// Outputs which will be evaluated
        /// </summary>
        public readonly IList<IOutput> Targets;

        /// <summary>
        /// Initialise a new instance
        /// </summary>
        public FileOutput(string filename, IList<IOutput> targets, bool append)
        {
            Filename = filename;
            Targets = targets;
            Append = append;
            if (Append)
            {
                _mustAppend = true;
            }
        }

        /// <summary>
        /// Evaluates all outputs.
        /// </summary>
        public void Evaluate()
        {
            using (var fileOut = new StreamWriter(Filename, _mustAppend))
            {
                Targets.ToList().ForEach(p => p.Evaluate(fileOut));
            }

            _mustAppend = true;
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }
    }
}
