namespace IISArt.Server
{
    using System;

    using IISArt.Abstractions;

    public class Logger : ILogger
    {
        public void Log(string logMessage, Exception exception)
        {
            if (exception == null)
            {
                Console.WriteLine($"{DateTime.Now}:{logMessage}");
            }
            else
            {
                Console.WriteLine($"{DateTime.Now}:{logMessage},{exception.Message}");
            }
        }
    }
}