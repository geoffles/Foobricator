using System.Collections.Generic;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class ListSequence : ISource, IDebugInfoProvider
    {
        public ListSequence(IList<object> listItems)
        {
            ListItems = listItems;
        }

        public ListSequence(DataReference reference)
        {
            ListItems = reference.Dereference() as IList<object>;
        }

        public IList<object> ListItems { get; set; }
        public DebugInfo DebugInfo { get; set; }

        private int _idx = 0; 

        public object GetItem()
        {
            var result = ListItems[_idx++];
            if (_idx >= ListItems.Count)
            {
                _idx = 0;
            }

            return result;
        }
    }
}
