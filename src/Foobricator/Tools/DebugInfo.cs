using System;

namespace Foobricator.Tools
{
    /// <summary>
    /// Class to provide debugging information.
    /// </summary>
    /// 
    public class DebugInfo : IFormattable
    {
        /// <summary>
        /// The path of the node
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The JSON node text
        /// </summary>
        public string NodeText { get; set; }

        /// <summary>
        /// Remarks about what the node (e.g. what is wrong with it)
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Get a text description of the debug info.
        /// </summary>
        /// <param name="format">See details for format options</param>
        /// <param name="formatProvider">Not used</param>
        /// <returns>Descriptive string for the info</returns>
        /// <para>
        /// Supplying "FULL" will give a detaild description of the Path and NodeText. Any other value 
        /// returns the default
        /// </para>        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                return DefaultFormat();
            }

            if (format == "FULL")
            {
                return string.Format("'{0}' ({1})", Path, NodeText);
            }

            return DefaultFormat();
        }

        /// <summary>
        /// Default formatting. Can be configured with <see cref="Foobricator.Tools.Log" />.
        /// </summary>
        /// <para>
        /// Outputs "FULL" when DebugInfoOnWarn is set.
        /// </para>
        private string DefaultFormat()
        {
            if (Log.Instance.DebugInfoOnWarn)
            {
                return string.Format("'{0}' ({1})", Path, NodeText);
            }
            else
            {
                return string.Concat("'", Path, "'");
            }            
        }
    }
}
