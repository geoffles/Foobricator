using System;

namespace Foobricator.Tools
{
    public interface ILog
    {
        bool DebugInfoOnWarn { get; set;  }

        void Info(string message);
        void Info(string message, params object[] formatArgs);
        void Trace(string message);
        void Trace(string message, params object[] formatArgs);
        void Warn(string message);
        void Warn(string message, params object[] formatArgs);
        void Error(string message);
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
            Info(string.Format(message, formatArgs));
        }
    }
}
