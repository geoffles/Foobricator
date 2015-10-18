using System;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    /// <summary>
    /// Value capture iterator
    /// </summary>
    /// <remarks>
    /// This allows you to capture random values and 
    /// reuse them, until you wish to reevaluate the sources.
    /// </remarks>
    public class Iterator: ISource, IDebugInfoProvider, IIterable
    {
        
        private static readonly Dictionary<string, List<IIterable>> _instances = new Dictionary<string, List<IIterable>>();

        /// <summary>
        /// List of iterables for iteration
        /// </summary>
        public static IReadOnlyList<IIterable> Instances { get { return _instances.SelectMany(p => p.Value).ToList().AsReadOnly(); } }

        /// <summary>
        /// Iteration scope
        /// </summary>
        public readonly string Scope;

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }
        
        /// <summary>
        /// The scope to use when none is provided
        /// </summary>
        public const string DefaultScope = "Global";

        /// <summary>
        /// Reevaluate all iterables in a matching scope
        /// </summary>
        /// <param name="scope"></param>
        public static void NextAll(string scope)
        {
            IList<string> inputScopes = scope.Split(':');

            List<string> relavanScopes = _instances
                .Keys
                .Where(key => key.Split(':').Intersect(inputScopes).Any())
                .Distinct()
                .ToList();

            relavanScopes.ForEach(x => _instances[x].ForEach(y => y.Next()));
           
        }

        /// <summary>
        /// The sources for this iterator
        /// </summary>
        public readonly IList<object> Sources;

        /// <summary>
        /// Captured values
        /// </summary>
        private IList<object> Items;

        /// <summary>
        /// Initialise
        /// </summary>
        public Iterator(IList<object> sources, string scope)
        {
            Scope = scope ?? DefaultScope;
            if (!_instances.ContainsKey(Scope))
            {
                _instances[Scope] = new List<IIterable>();
            }
            _instances[Scope].Add(this);

            Sources = sources.Select(p => p is DataReference
                ? ((DataReference) p).Dereference()
                : p)
                .ToList();
            Next();
        }

        /// <summary>
        /// Capture the next value
        /// </summary>
        public void Next()
        {
            Items = Sources
                .Select(GetItems)
                .Select(p => p is IList<object>
                    ? (IList<object>)p
                    : new List<object> { p })
                .SelectMany(p => p)
                .ToList();
        }

        private object GetItems(object p)
        {
            object result = p;

            if (result is DataReference)
            {
                var dref  = ((DataReference) result).Dereference();
                result = dref;
            }

            if (result is ISource)
            {
                result = ((ISource) result).GetItem();
            }

            return result;
        }

        /// <summary>
        /// Returns all items in the iterator. From <see cref="Foobricator.Sources.ISource"/>
        /// </summary>
        public object GetItem()
        {
            return Items;
        }

        /// <summary>
        /// Remove this iterator from the list of iterables
        /// </summary>
        public void Dispose()
        {
            _instances[Scope].Remove(this);
        }
    }
}
