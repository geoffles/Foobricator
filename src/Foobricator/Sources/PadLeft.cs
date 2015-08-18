using System;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    class PadLeft : ISource, IDebugInfoProvider
    {
        public readonly int Length;
        public readonly char Character;
        public readonly ISource Source;

        public PadLeft(DataReference reference, int length, char character)
            : this(reference.Dereference().First() as ISource, length, character)
        {

        }

        public PadLeft(ISource source, int length, char character)
        {
            Source = source;
            Character = character;
            Length = length;
        }

        public DebugInfo DebugInfo { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            object value = Source.GetItem();

            string str = value is string
                ? (string)value
                : value.ToString();

            return str.PadLeft(Length, Character);
        }

        public object GetItem()
        {
            object value = Source.GetItem();

            string str = value is string
                ? (string)value
                : value.ToString();

            return str.PadLeft(Length, Character);
        }
    }
}
