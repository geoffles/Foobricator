using System;

namespace Foobricator.Tools
{
    class Log
    {
        public static Log Instance = new Log();

        private Log()
        {
            DebugInfoOnWarn = false;
        }

        public bool DebugInfoOnWarn { get; set; }

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
    }
}
