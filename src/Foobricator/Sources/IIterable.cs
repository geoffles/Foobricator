using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foobricator.Sources
{
    /// <summary>
    /// Interface for object that can be iterated
    /// </summary>
    public interface IIterable : IDisposable
    {
        /// <summary>
        /// Iterate
        /// </summary>
        void Next();
    }
}
