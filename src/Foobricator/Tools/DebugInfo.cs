using System;

namespace Foobricator.Tools
{
    public class DebugInfo : IFormattable
    {
        public string Path { get; set; }
        public string NodeText { get; set; }
        public string Comment { get; set; }
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
