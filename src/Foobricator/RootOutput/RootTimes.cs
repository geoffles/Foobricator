using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobricator.Output;
using Foobricator.Sources;
using Foobricator.Tools;

namespace Foobricator.RootOutput
{
    public class RootTimes : IRootOutput, IDebugInfoProvider
    {
        public readonly List<IRootOutput> Target;
        public readonly int Count;
        public readonly string Separator;
        public readonly string Scope;

        public RootTimes(IEnumerable<IRootOutput> target, int count, string separator, string scope)
        {
            Target = target.ToList();
            Count = count;
            Scope = scope ?? Iterator.DefaultScope;
        }

        public DebugInfo DebugInfo { get; set; }

        public void Evaluate()
        {
            for (int i = 0; i < Count; i++)
            {
                Target.ForEach(p => p.Evaluate());
                Iterator.NextAll(Scope);
            }
        }      
    }
}
