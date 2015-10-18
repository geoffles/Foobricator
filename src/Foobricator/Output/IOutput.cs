using System.IO;

namespace Foobricator.Output
{
    /// <summary>
    /// Interface for an output node
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// Implementing classes must evaluate and emit to the text writer
        /// </summary>
        void Evaluate(TextWriter writer);
    }
}
