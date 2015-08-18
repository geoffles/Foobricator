using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foobricator.Output;
using Foobricator.Tools;

namespace Foobricator.RootOutput
{
    public class FileOutput : IRootOutput, IDebugInfoProvider
    {
        public readonly string Filename;
        public readonly IList<IOutput> Targets;

        public FileOutput(string filename, IList<IOutput> targets)
        {
            Filename = filename;
            Targets = targets;
        }

        public void Evaluate()
        {
            using (var fileOut = new StreamWriter(Filename))
            {
                Targets.ToList().ForEach(p => p.Evaluate(fileOut));
            }
        }

        public DebugInfo DebugInfo { get; set; }
    }
}
