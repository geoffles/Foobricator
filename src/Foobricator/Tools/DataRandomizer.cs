using System;

namespace Foobricator.Tools
{
    /// <summary>
    /// Interface for random data - allows injection.
    /// </summary>
    public interface IDataRandomizer
    {
        /// <summary>
        /// Upper case Char
        /// </summary>
        char UpperChar();
        
        /// <summary>
        /// Lower case Char
        /// </summary>
        char LowerChar();

        /// <summary>
        /// Decimal Char
        /// </summary>
        char DecimalChar();
        
        /// <summary>
        /// Random bool
        /// </summary>
        bool RandomBool();

        /// <summary>
        /// Random Integer
        /// </summary>
        int RandomInt();

        /// <summary>
        /// Random Double
        /// </summary>
        double RandomDouble();
    }

    /// <summary>
    /// Singleton. Basic implementation of Data Randomizer.
    /// </summary>
    public class DataRandomizer : IDataRandomizer
    {
        private DataRandomizer()
        {
        }

        private Random _random = new Random();
        
        private static IDataRandomizer _instance = new DataRandomizer();
        
        /// <summary>
        /// The current single
        /// </summary>
        public static IDataRandomizer Instance { get { return _instance; } }

        /// <summary>
        /// Upper case Char
        /// </summary>
        public char UpperChar() { return (char)(_random.Next() % 26 + 65); }

        /// <summary>
        /// Lower case Char
        /// </summary>
        public char LowerChar() { return (char)(_random.Next() % 26 + 97); }

        /// <summary>
        /// Decimal Char
        /// </summary>
        public char DecimalChar() { return (char)(_random.Next() % 10 + 48); }

        /// <summary>
        /// Random Bool
        /// </summary>
        public bool RandomBool() { return _random.Next() % 2 == 0; }

        /// <summary>
        /// Random Integer
        /// </summary>
        public int RandomInt() { return _random.Next(); }

        /// <summary>
        /// Random Double
        /// </summary>
        public double RandomDouble() { return _random.NextDouble(); }
    } 
}
