using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Output
{
    /// <summary>
    /// Only output the targets when a condition is met
    /// </summary>
    public class ConditionalOutput : IOutput, IDebugInfoProvider
    {
        /// <summary>
        /// The condition on which to output the targets
        /// </summary>
        public readonly When When;

        /// <summary>
        /// A list of targets which will be output when the condition is met
        /// </summary>
        public readonly List<IOutput> Target;

        /// <summary>
        /// Create a new conditional output
        /// </summary>
        /// <param name="target">Target outputs</param>
        /// <param name="when">Condition for output</param>
        public ConditionalOutput(IEnumerable<IOutput> target, When when)
        {
            When = when;
            Target = target.ToList();
        }

        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Tests the when condition and evaluates the outputs when true.
        /// </summary>
        /// <param name="writer"></param>
        public void Evaluate(TextWriter writer)
        {
            if (When.True())
            {
                Target.ForEach(p => p.Evaluate(writer));
            }
        }
    }
}
