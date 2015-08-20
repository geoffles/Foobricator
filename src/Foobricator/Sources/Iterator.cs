using System;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class Iterator: ISource, IDisposable, IDebugInfoProvider
    {
        private static readonly Dictionary<string, List<Iterator>> _instances = new Dictionary<string, List<Iterator>>();
        public static IReadOnlyList<Iterator> Instances { get { return _instances.SelectMany(p => p.Value).ToList().AsReadOnly(); } }
        public readonly string Scope;

        public DebugInfo DebugInfo { get; set; }
        public const string DefaultScope = "Global";

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

        public readonly IList<object> Sources;
        private IList<object> Items;

        public Iterator(IList<object> sources, string scope)
        {
            Scope = scope ?? DefaultScope;
            if (!_instances.ContainsKey(Scope))
            {
                _instances[Scope] = new List<Iterator>();
            }
            _instances[Scope].Add(this);

            Sources = sources.Select(p => p is DataReference
                ? ((DataReference) p).Dereference()
                : p)
                .ToList();
            Next();
        }

        private void Next()
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

        public object GetItem()
        {
            return Items;
        }

        public void Dispose()
        {
            _instances[Scope].Remove(this);
        }
    }
}
