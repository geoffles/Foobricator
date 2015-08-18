using System.Collections.Generic;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    class TupleValue : ISource, IDebugInfoProvider
    {
        public readonly ISource Source;
        public readonly int Index;

        public TupleValue(DataReference reference, int index)
        {
            var list = reference.Dereference();

            if (list.Count == 1)
            {
                Source = list[0] as ISource;
            }

            Index = index;
        }

        public TupleValue(ISource sourceList, int index)
        {
            Source = sourceList;
            Index = index;
        }

        public DebugInfo DebugInfo { get; set; }

        public object GetItem()
        {

            if (Source == null)
            {
                return null;
            }

            IList<object> value = Source.GetItem() as IList<object>;

            if (value == null)
                return null;

            if (Index < value.Count)
            {
                return value[Index];
            }

            return null;
        }
    }
}
