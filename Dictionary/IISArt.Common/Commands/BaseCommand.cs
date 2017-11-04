namespace IISArt.Common.Commands
{
    using System;
    using System.Linq;

    using IISArt.Abstractions;

    public class BaseCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        public BaseCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        protected BaseCommand(string[] args)
        {
            Init(args);
        }

        private void Init(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentNullException($"Can't build command. Invalid arguments.");
            }

            Key = args.FirstOrDefault();

            if (string.IsNullOrEmpty(Key))
            {
                throw new ArgumentNullException($"Value for Key is not presented");
            }

            Values = args.Skip(1).ToArray();
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        public string[] Values { get; private set; }

        /// <summary>
        /// The execute this command.
        /// Idle by default.
        /// </summary>
        /// <param name="wordDictionary">
        /// The word dictionary.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public virtual string Execute(IWordDictionary wordDictionary)
        {
            return string.Empty;
        }

        public void Build(string[] args)
        {
            Init(args);
        }

        public virtual bool IsValid()
        {
            if (string.IsNullOrEmpty(Key))
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            var result = $"{Key} " + string.Join(" ", Values);
            return result;
        }
    }
}