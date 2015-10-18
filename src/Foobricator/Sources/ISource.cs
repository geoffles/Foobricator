namespace Foobricator.Sources
{
    /// <summary>
    /// Interface to represent a data source
    /// </summary>
    public interface ISource
    {
        /// <summary>
        /// Implementing classes perform an evaluation
        /// </summary>
        /// <returns>Whatever is eveluated</returns>
        object GetItem();
    }
}
