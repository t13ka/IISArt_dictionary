namespace IISArt.Server
{
    using System;
    using System.Net.Sockets;
    using System.Threading;

    using IISArt.Abstractions;
    using IISArt.Common;
    using IISArt.Server.NinjectIoc;

    using Ninject;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Dictionary server starting...");

            var registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            var wordDictionary = kernel.Get<IWordDictionary>();
            var commandBuilder = kernel.Get<ICommandBuilder>();
            var logger = kernel.Get<ILogger>();

            TcpListener listener = null;

            try
            {
                var connectionInfo = ConnectionUtils.ParseConnectionParams(args);
                listener = new TcpListener(connectionInfo.IpAddress, connectionInfo.Port);
                listener.Start();
                Console.WriteLine($"Waiting for connections at {connectionInfo.IpAddress}:{connectionInfo.Port}");

                while (true)
                {
                    var client = listener.AcceptTcpClient();

                    var clientObject = new ClientObject(client, wordDictionary, commandBuilder, logger);

                    var action = new Action(clientObject.Process);

                    ThreadPool.QueueUserWorkItem(t => action());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                listener?.Stop();

                Console.ReadKey();
            }
        }
    }
}