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

            Source = list as ISource;

            Index = index;
        }

        public TupleValue(ISource sourceList, int index)
        {
            Source = sourceList;
            Index = index;
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Gets the object at <c>Index</c> for the <c>Source</c> list. From <see cref="Foobricator.Sources.ISource"/>
        /// </summary>
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
