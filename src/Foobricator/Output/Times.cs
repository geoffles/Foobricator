using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foobricator.Sources;
using Foobricator.Tools;

namespace Foobricator.Output
{
    public class Times : IOutput, IDebugInfoProvider
    {
        public readonly List<IOutput> Target;
        public readonly int Count;
        public readonly string Separator;

        public Times(IEnumerable<IOutput> target, int count, string separator)
        {
            Target = target.ToList();
            Count = count;
            Separator = separator;
        }


        public Times(IOutput target, int count, string separator)
        {
            Target = new List<IOutput>{target};
            Count = count;
            Separator = separator;
        }

        public DebugInfo DebugInfo { get; set; }

        public void Evaluate(TextWriter writer)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Separator != null && i > 0)
                {
                    writer.Write(Separator);
                }
                Target.ForEach(p => p.Evaluate(writer));
                Iterator.NextAll();
            }
        }
    }
}
