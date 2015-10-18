using System;

namespace Foobricator.Tools
{
    /// <summary>
    /// Interface for foobricator logging
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Include high detail on warnings
        /// </summary>
        bool DebugInfoOnWarn { get; set;  }

        /// <summary>
        /// Print an information message
        /// </summary>
        void Info(string message);
        
        /// <summary>
        /// Print a format string information message
        /// </summary>        
        void Info(string message, params object[] formatArgs);

        /// <summary>
        /// Print a trace message
        /// </summary>
        void Trace(string message);

        /// <summary>
        /// Print a format string trace message
        /// </summary>
        void Trace(string message, params object[] formatArgs);

        /// <summary>
        /// Print a warning message
        /// </summary>
        void Warn(string message);
        
        /// <summary>
        /// Print a format string warning message
        /// </summary>
        void Warn(string message, params object[] formatArgs);

        /// <summary>
        /// Print an error message
        /// </summary>
        void Error(string message);
        
        /// <summary>
        /// Print a format string error message
        /// </summary>
        void Error(string message, params object[] formatArgs);
    }

    class Log : ILog
    {
        public static ILog Instance = new Log();

        private Log()
        {
            DebugInfoOnWarn = false;
        }

        public bool DebugInfoOnWarn { get; set; }

        public void Trace(string message)
        {
            Console.WriteLine(message);
        }

        public void Trace(string message, params object[] formatArgs)
        {
            Info(string.Format(message, formatArgs));
        }

        public void Info(string message)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Cyan;
            
            Console.WriteLine(message);

            Console.ForegroundColor = color;
        }

        public void Info(string message, params object[] formatArgs)
        {
            Info(string.Format(message, formatArgs));
        }

        public void Warn(string message)
        {
            var cursorTop = Console.CursorTop;
            var cursorLeft = Console.CursorLeft;

            //Console.SetCursorPosition(0, cursorTop+1);

            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            

            Console.WriteLine(message);

            Console.ForegroundColor = color;
            //Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        public void Warn(string message, params object[] formatArgs)
        {
            Warn(string.Format(message, formatArgs));
        }

        public void Error(string message)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(message);

            Console.ForegroundColor = color;
        }

        public void Error(string message, params object[] formatArgs)
        {
            Error(string.Format(message, formatArgs));
        }
    }
}
