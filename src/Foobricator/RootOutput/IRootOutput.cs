namespace Foobricator.RootOutput
{
    /// <summary>
    /// Interface for a top level output
    /// </summary>
    public interface IRootOutput
    {
        /// <summary>
        /// Implementing classes must begin their output
        /// </summary>
        void Evaluate();
    }
}
