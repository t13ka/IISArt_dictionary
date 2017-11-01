namespace IISArt.Common
{
    using System.Net;

    using IISArt.Abstractions;

    public class ConnectionInfo : IConnectionInfo
    {
        private const int DefaultPort = 9999;

        private const string DefaultAddress = "127.0.0.1";

        public ConnectionInfo()
        {
            IpAddress = IPAddress.Parse(DefaultAddress);
            Port = DefaultPort;
        }

        public ConnectionInfo(IPAddress ipAddress)
        {
            IpAddress = ipAddress;
            Port = DefaultPort;
        }

        public ConnectionInfo(IPAddress ipAddress, bool parsed)
        {
            IpAddress = ipAddress;
            Port = DefaultPort;
            IsParsed = parsed;
        }

        public IPAddress IpAddress { get; }

        public int Port { get; }

        public bool IsParsed { get; }
    }
}