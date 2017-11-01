namespace IISArt.Common
{
    using System.Net;

    using IISArt.Abstractions;

    public static class ConnectionUtils
    {
        public static IConnectionInfo ParseConnectionParams(string[] args)
        {
            ConnectionInfo result;

            if (args.Length > 0 && string.IsNullOrEmpty(args[0]) == false)
            {
                var ipAddress = IPAddress.Parse(args[0]);
                result = new ConnectionInfo(ipAddress, true);
            }
            else
            {
                result = new ConnectionInfo();
            }

            return result;
        }
    }
}