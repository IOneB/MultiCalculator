using RestServer.Services.Interface;
using System;

namespace RestServer
{
    /// <summary>
    /// Реализация кастомного логгера для записи в консоль
    /// </summary>
    internal class ConsoleLog : ILog
    {
        private void Log(string message, string title, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"[{DateTime.UtcNow.ToString()}][{title}]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public void Debug(string message)
        {
            Log(message, "Debug", ConsoleColor.Cyan);
        }

        public void Error(string message)
        {
            Log(message, "Error", ConsoleColor.Red);
        }

        public void Info(string message)
        {
            Log(message, "Info", ConsoleColor.Green);
        }
    }
}