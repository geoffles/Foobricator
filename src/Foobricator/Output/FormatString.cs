using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Foobricator.Sources;
using Foobricator.Tools;

namespace Foobricator.Output
{
    /// <summary>
    /// Output a cusotm formatted string against an source data set
    /// </summary>
    public class FormatString : IOutput, IDebugInfoProvider
    {
        /// <summary>
        /// A standard .Net format string
        /// </summary>
        public readonly string Format;

        /// <summary>
        /// Reference to the source dataset
        /// </summary>
        public readonly object Source;

        public readonly bool SuppressEndLine;
        
        /// <summary>
        /// Create a new instance 
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="formatString"></param>
        /// <example>{type:"formatString", format:"First{0}|Second{1}", source:{type:"reference", refersTo:"mySource"} }</example>
        public FormatString(DataReference reference, string formatString, bool suppressEndLine)
        {
            Source = reference;
            Format = formatString;
            SuppressEndLine = suppressEndLine;
        }

        public FormatString(ISource source, string formatString, bool suppressEndLine)
        {
            Source = source;
            Format = formatString;
            SuppressEndLine = suppressEndLine;
        }

        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Evaluates the source reference against the format string.
        /// </summary>
        public void Evaluate(TextWriter writer)
        {
            object row = Source;

            if (row is DataReference)
            {
                row = ((DataReference)row).Dereference();
            }

            if (row is ISource)
            {
                row = ((ISource) row).GetItem();
            }

            writer.Write(string.Format(CultureInfo.InvariantCulture, Format + (SuppressEndLine ? string.Empty: Environment.NewLine), ((IList<object>)row).ToArray()));  
        }

    }
}
