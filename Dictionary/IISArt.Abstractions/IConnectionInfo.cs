namespace IISArt.Abstractions
{
    using System.Net;

    public interface IConnectionInfo
    {
        IPAddress IpAddress { get; }

        int Port { get; }

        bool IsParsed { get; }
    }
}