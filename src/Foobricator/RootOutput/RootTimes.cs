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
    /// <summary>
    /// A repeater output for root level. Creates a textwriter.
    /// </summary>
    public class RootTimes : IRootOutput, IDebugInfoProvider
    {
        /// <summary>
        /// The list of root outputs
        /// </summary>
        public readonly List<IRootOutput> Target;

        /// <summary>
        /// Number of times to repeat
        /// </summary>
        public readonly int Count;

        /// <summary>
        /// What to emit between repeats
        /// </summary>
        public readonly string Separator;

        /// <summary>
        /// The iterator scope
        /// </summary>
        public readonly string Scope;

        /// <summary>
        /// Initaliase a new instance
        /// </summary>        
        public RootTimes(IEnumerable<IRootOutput> target, int count, string separator, string scope)
        {
            Target = target.ToList();
            Count = count;
            Scope = scope ?? Iterator.DefaultScope;
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Iterates <c>Count</c> times.
        /// </summary>
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
