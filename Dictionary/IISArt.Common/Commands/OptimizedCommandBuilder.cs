namespace IISArt.Common.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using IISArt.Abstractions;

    public class OptimizedCommandBuilder : ICommandBuilder
    {
        /// <summary>
        /// The available commands.
        /// </summary>
        private static readonly Dictionary<string, ICommand> AvailableCommands;

        static OptimizedCommandBuilder()
        {
            AvailableCommands = new Dictionary<string, ICommand>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assambly = assemblies.First(t => t.FullName.Contains("IISArt.Common"));
            var types = assambly.GetTypes();
            var baseCommandTypeName = typeof(BaseCommand).Name;

            foreach (var t in types.Where(t => t.BaseType != null && t.BaseType.Name == baseCommandTypeName))
            {
                var name = t.Name.ToLower();
                var commandName = name.Replace("command", string.Empty);
                if (AvailableCommands.ContainsKey(commandName) == false)
                {
                    var instance = Activator.CreateInstance(t);
                    AvailableCommands.Add(commandName, instance as ICommand);
                }
            }
        }

        public ICommand Build(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException($"Empty message");
            }

            var args = message.Split(' ');

            return Build(args);
        }

        public ICommand Build(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentNullException($"Can't parse command by presented arguments.");
            }

            if (string.IsNullOrEmpty(args[1]))
            {
                throw new ArgumentNullException($"Keyword is not presented.");
            }

            // check if this an IP address
            var cmdIndex = 0;
            if (IPAddress.TryParse(args[cmdIndex], out IPAddress ipAddress))
            {
                cmdIndex++;
            }

            var attemptedCommand = args[cmdIndex];
            if (AvailableCommands.TryGetValue(attemptedCommand, out ICommand command))
            {
                var commandArgs = args.Skip(1).ToArray(); // skip first command word
                command.Build(commandArgs);
                return command;
            }

            throw new ArgumentNullException($"Command \"{attemptedCommand}\" is not supported.");
        }
    }
}