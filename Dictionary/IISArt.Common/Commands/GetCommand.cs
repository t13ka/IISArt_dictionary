namespace IISArt.Common.Commands
{
    using System;
    using System.Text;

    using IISArt.Abstractions;

    public class GetCommand : BaseCommand
    {
        public GetCommand(string[] args)
            : base(args)
        {
        }

        public override string Execute(IWordDictionary wordDictionary)
        {
            var result = wordDictionary.Get(Key);

            if (result.Length > 0)
            {
                var sb = new StringBuilder();
                sb.Append(string.Join(Environment.NewLine, result));
                return sb.ToString();
            }
            else
            {
                return $"a word \"{Key}\" is missing in dictionary";
            }
        }
    }
}