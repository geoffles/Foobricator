using System;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class Iterator: ISource, IDisposable, IDebugInfoProvider
    {
        private static readonly List<Iterator> _instances = new List<Iterator>();
        public DebugInfo DebugInfo { get; set; }

        public static void NextAll()
        {
            _instances.ForEach(p => p.Next());
        }

        public readonly IList<object> Sources;
        private IList<object> Items;

        private Iterator()
        {
            _instances.Add(this);
        }

        public Iterator(IList<object> sources) : this()
        {
            Sources = sources.Select(p => p is DataReference ? ((DataReference)p).Dereference().First() : p).ToList();
            Next();
        }

        public Iterator(IEnumerable<DataReference> references)
            : this(references.Select(p => p.Dereference()).Select(p => p.First()).ToList())
        { }

        public Iterator(DataReference reference) : this(new List<object>{reference.Dereference().First()})
        {}

        public Iterator(ISource source) : this(new List<object>{source})
        {}

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

                if (dref.Count == 1)
                {
                    result = dref[0];
                }
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
