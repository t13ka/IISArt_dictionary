namespace IISArt.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using IISArt.Abstractions;
    using IISArt.Common;
    using IISArt.Common.Commands;

    using Ninject;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Dictionary server starting...");

            var kernel = new StandardKernel();
            kernel.Bind<IWordDictionary>().To<WordDictionary>().InSingletonScope();
            kernel.Bind<ICommandParser>().To<CommandParser>().InSingletonScope();
            kernel.Bind<ILogger>().To<Logger>().InSingletonScope();

            var wordDictionary = kernel.Get<IWordDictionary>();
            var commandParser = kernel.Get<ICommandParser>();
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

                    var clientObject = new ClientObject(client, wordDictionary, commandParser, logger);

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