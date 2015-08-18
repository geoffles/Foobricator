using System.Collections.Generic;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class ListSampler : ISource, IDebugInfoProvider
    {
        public ListSampler(IList<object> listItems)
        {
            ListItems = listItems;
        }

        public ListSampler(DataReference reference)
        {
            ListItems = reference.Dereference();
        }

        public int Count { get; set; }
        public IList<object> ListItems { get; set; }
        public DebugInfo DebugInfo { get; set; }

        public object GetItem()
        {
            var idx = DataRandomizer.Instance.RandomInt() % (ListItems.Count);
            return ListItems[idx];
        }
    }
}
