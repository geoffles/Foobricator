using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foobricator.Sources;
using Foobricator.Tools;

namespace Foobricator.Output
{
    /// <summary>
    /// A repeater output.
    /// </summary>
    public class Times : IOutput, IDebugInfoProvider
    {
        /// <summary>
        /// List of outputs
        /// </summary>
        public readonly List<IOutput> Target;

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
        /// Initialise a new instance
        /// </summary>
        public Times(IEnumerable<IOutput> target, int count, string separator, string scope)
        {
            Target = target.ToList();
            Count = count;
            Separator = separator;
            Scope = scope ?? Iterator.DefaultScope;
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Iterates <c>Count</c> times
        /// </summary>
        /// <param name="writer"></param>
        public void Evaluate(TextWriter writer)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Separator != null && i > 0)
                {
                    writer.Write(Separator);
                }
                Target.ForEach(p => p.Evaluate(writer));
                Iterator.NextAll(Scope);
            }
        }
    }
}
