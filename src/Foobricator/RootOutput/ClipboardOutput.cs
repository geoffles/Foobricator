using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foobricator.Output;
using Foobricator.Tools;

namespace Foobricator.RootOutput
{
    class ClipboardOutput : IRootOutput, IDebugInfoProvider
    {
        public readonly IList<IOutput> Targets;

        public ClipboardOutput(IList<IOutput> targets)
        {
            Targets = targets;
        }

        public void Evaluate()
        {
            using (var stringWriter = new StringWriter())
            {
                Targets.ToList().ForEach(p => p.Evaluate(stringWriter));

                System.Windows.Forms.Clipboard.SetText(stringWriter.ToString());
            }

            Log.Instance.Info("Output copied to clipboard.");
        }

        public DebugInfo DebugInfo { get; set; }
    }
}
