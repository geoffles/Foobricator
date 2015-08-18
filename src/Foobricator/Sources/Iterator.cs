using System;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class Iterator: ISource, IDisposable, IDebugInfoProvider
    {
        private static readonly List<Iterator> _instances = new List<Iterator>();
        public static IReadOnlyList<Iterator> Instances { get { return _instances.AsReadOnly(); } }

        public DebugInfo DebugInfo { get; set; }

        public static void NextAll()
        {
            _instances.ForEach(p => p.Next());
        }

        public readonly IList<object> Sources;
        private IList<object> Items;

        public Iterator(IList<object> sources)
        {
            _instances.Add(this);
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
            _instances.Remove(this);
        }
    }
}
