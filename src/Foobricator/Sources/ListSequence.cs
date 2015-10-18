using System.Collections.Generic;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    /// <summary>
    /// List which is evaluated sequentiallly
    /// </summary>
    public class ListSequence : ISource, IDebugInfoProvider
    {
        /// <summary>
        /// Initalise against a list
        /// </summary>
        public ListSequence(IList<object> listItems)
        {
            ListItems = listItems;
        }

        /// <summary>
        /// Initialise against a reference
        /// </summary>
        public ListSequence(DataReference reference)
        {
            ListItems = reference.Dereference() as IList<object>;
        }

        /// <summary>
        /// The list of items
        /// </summary>
        public IList<object> ListItems { get; set; }
        
        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        private int _idx = 0;

        /// <summary>
        /// Gets the next value in the sequence. From <see cref="Foobricator.Sources.ISource"/>
        /// </summary>
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
