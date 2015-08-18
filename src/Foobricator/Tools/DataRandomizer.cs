using System;

namespace Foobricator.Tools
{
    public interface IDataRandomizer
    {
        char UpperChar();
        char LowerChar();
        char DecimalChar();
        bool RandomBool();
        int RandomInt();
        double RandomDouble();
    }

    public class DataRandomizer : IDataRandomizer
    {
        private DataRandomizer()
        {
        }

        private static IDataRandomizer _instance = new DataRandomizer();
        public static IDataRandomizer Instance { get { return _instance; } }

        private Random _random = new Random();

        public char UpperChar() { return (char)(_random.Next() % 26 + 65); }
        public char LowerChar() { return (char)(_random.Next() % 26 + 97); }
        public char DecimalChar() { return (char)(_random.Next() % 10 + 48); }
        public bool RandomBool() { return _random.Next() % 2 == 0; }
        public int RandomInt() { return _random.Next(); }
        public double RandomDouble() { return _random.NextDouble(); }
    } 
}
