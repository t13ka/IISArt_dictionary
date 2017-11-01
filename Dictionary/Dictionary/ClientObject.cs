﻿using System;
using System.Text;

namespace IISArt.Server
{
    using System.Net.Sockets;

    using IISArt.Abstractions;

    public class ClientObject
    {
        private readonly TcpClient _tcpClient;

        private readonly IWordDictionary _wordDictionary;

        private readonly ICommandParser _commandParser;

        private readonly ILogger _logger;

        private const int MessageLength = 1024; 

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientObject"/> class.
        /// </summary>
        /// <param name="tcpTcpClient">
        /// The tcp tcp client.
        /// </param>
        /// <param name="wordDictionary">
        /// The word dictionary.
        /// </param>
        /// <param name="commandParser">
        /// The command parser.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public ClientObject(
            TcpClient tcpTcpClient,
            IWordDictionary wordDictionary,
            ICommandParser commandParser,
            ILogger logger)
        {
            _tcpClient = tcpTcpClient;
            _wordDictionary = wordDictionary;
            _commandParser = commandParser;
            _logger = logger;
        }

        public void Process()
        {
            NetworkStream stream = null;

            try
            {
                stream = _tcpClient.GetStream();
                var data = new byte[MessageLength];

                while (true)
                {
                    var builder = new StringBuilder();

                    do
                    {
                        var bytes = stream.Read(data, 0, data.Length);

                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    var message = builder.ToString();
                    if (string.IsNullOrEmpty(message))
                    {
                        // idle
                        continue;
                    }

                    var cmd = _commandParser.Parse(message);
                    if (cmd != null)
                    {
                        if (cmd.IsValid())
                        {
#if DEBUG
                            _logger.Log($"request: command:{cmd.GetType().Name}, params:{cmd}");
#endif

                            var result = cmd.Execute(_wordDictionary);

#if DEBUG
                            _logger.Log($"response:{result}");
#endif

                            data = Encoding.Unicode.GetBytes(result);
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            _logger.Log($"Command \"{cmd.GetType().Name}\" is invalid!");
                        }
                    }
                    else
                    {
                        _logger.Log($"Can't parse command by presented args");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Error:", ex);
            }
            finally
            {
                _tcpClient?.Close();
                stream?.Close();
            }
        }
    }
}