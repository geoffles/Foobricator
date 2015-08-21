using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foobricator.Output;
using Foobricator.Tools;

namespace Foobricator.RootOutput
{
    public class FileOutput : IRootOutput, IDebugInfoProvider
    {
        public readonly bool Append;
        private bool _mustAppend = false;
        public readonly string Filename;
        public readonly IList<IOutput> Targets;

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

        public void Evaluate()
        {
            using (var fileOut = new StreamWriter(Filename, _mustAppend))
            {
                Targets.ToList().ForEach(p => p.Evaluate(fileOut));
            }

            _mustAppend = true;
        }

        public DebugInfo DebugInfo { get; set; }
    }
}
