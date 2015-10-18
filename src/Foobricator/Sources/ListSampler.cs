using System.Collections.Generic;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    /// <summary>
    /// Pulls a random value from a list
    /// </summary>
    public class ListSampler : ISource, IDebugInfoProvider
    {
        /// <summary>
        /// Initialise against a list
        /// </summary>
        public ListSampler(IList<object> listItems)
        {
            ListItems = listItems;
        }

        /// <summary>
        /// Initialise against a <see cref="Foobricator.Tools.DataReference"/>
        /// </summary>
        /// <param name="reference"></param>
        public ListSampler(DataReference reference)
        {
            ListItems = reference.Dereference() as IList<object>;
        }

        /// <summary>
        /// The list that will be sampled from
        /// </summary>
        public IList<object> ListItems { get; set; }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Gets a single item from the <c>ListItems</c>. From <see cref="Foobricator.Sources.ISource"/>
        /// </summary>
        public object GetItem()
        {
            var idx = DataRandomizer.Instance.RandomInt() % (ListItems.Count);
            return ListItems[idx];
        }
    }
}
