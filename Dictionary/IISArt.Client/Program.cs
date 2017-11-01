namespace IISArt.Client
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    using IISArt.Abstractions;
    using IISArt.Common;
    using IISArt.Common.Commands;

    using Ninject;

    class Program
    {
        private const int MessageLength = 1024;

        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Bind<ICommandParser>().To<CommandParser>().InSingletonScope();
            var commandParser = kernel.Get<ICommandParser>();
            var command = commandParser.Parse(args);
            if (command.IsValid() == false)
            {
                throw new ArgumentNullException($"Command is invalid");
            }

            var flatCommand = command.ToString();

            TcpClient client = null;
            NetworkStream stream = null;

            try
            {
                var connection = ConnectionUtils.ParseConnectionParams(args);

                client = new TcpClient(connection.IpAddress.ToString(), connection.Port);

                // request
                stream = client.GetStream();
                var requestBytes = Encoding.Unicode.GetBytes(flatCommand);
                stream.Write(requestBytes, 0, requestBytes.Length);

                // response
                var responseBytes = new byte[MessageLength];
                var builder = new StringBuilder();
                do
                {
                    var bytes = stream.Read(responseBytes, 0, responseBytes.Length);
                    builder.Append(Encoding.Unicode.GetString(responseBytes, 0, bytes));
                }
                while (stream.DataAvailable);

                Console.WriteLine("{0}", builder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client?.Close();
                stream?.Close();
            }

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}