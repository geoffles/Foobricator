using System.IO;

namespace Foobricator.Output
{
    public interface IOutput
    {
        void Evaluate(TextWriter writer);
    }
}
