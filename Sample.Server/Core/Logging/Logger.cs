using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;


namespace Sample.Server.Core.Logging
{
    /// <summary>
    /// Logger Utility https://gist.github.com/headdetect/3281887
    /// </summary>
    public class Logger
    {
        public static bool LogsEnabled = true;
        public static void Log(string message, LogType logType = LogType.Normal)
        {
            if (!LogsEnabled)
                return;
            ConsoleColor one = ConsoleColor.Green;
            switch (logType)
            {
                case LogType.Critical:
                    one = ConsoleColor.DarkRed;
                    break;
                case LogType.Debug:
                    one = ConsoleColor.DarkMagenta;
                    break;
                case LogType.Error:
                    one = ConsoleColor.Red;
                    break;
                case LogType.Warning:
                    one = ConsoleColor.Yellow;
                    break;
            }
            Log(message, one, logType);
        }

        public static void Log(string message, ConsoleColor textColor, LogType logType = LogType.Normal)
        {
            if (!LogsEnabled)
                return;
            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Logs an exception, to be grabbed by a log event handler
        /// </summary>
        /// <param name="e">Exception to be logged</param>
        public static void LogError(Exception e)
        {
            Log(e.Message + "\n" + e.StackTrace, LogType.Error);
        }
    }

    /// <summary>
    /// Log type for the specified message
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// The normal messages
        /// </summary>
        Normal,
        /// <summary>
        /// Error messages
        /// </summary>
        Error,
        /// <summary>
        /// Debug messages (only appears if the program is in debugging mode)
        /// </summary>
        Debug,
        /// <summary>
        /// Warning messages
        /// </summary>
        Warning,
        /// <summary>
        /// Critical messages
        /// </summary>
        Critical,

    }

    /// <summary>
    ///Log event where object holding the event
    ///would get a string (the message)
    /// </summary>
    public class LogEventArgs : EventArgs
    {

        /// <summary>
        /// Get or set the message of the log event
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Get or set the type of log
        /// </summary>
        public LogType LogType { get; set; }


        public LogEventArgs(string log, LogType logType)
        {
            Message = log;
            LogType = logType;
        }
    }
}