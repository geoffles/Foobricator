using Foobricator.Sources;

namespace Foobricator.Tests.TestTools
{
    internal class TestSource : ISource
    {
        public int Count = 0;

        public object GetItem()
        {
            return Count++;
        }
    }
}