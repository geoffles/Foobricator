using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foobricator.Sources
{
    public interface IIterable : IDisposable
    {
        void Next();
    }
}
