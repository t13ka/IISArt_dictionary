namespace IISArt.Abstractions
{
    using System;

    public interface ILogger
    {
        void Log(string logMessage, Exception exception = null);
    }
}