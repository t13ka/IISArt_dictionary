namespace IISArt.Common.Commands
{
    using System.Linq;
    using System.Text;

    using IISArt.Abstractions;

    public class AddCommand : BaseCommand
    {
        public AddCommand()
            : base()
        {
        }

        public AddCommand(string[] args)
            : base(args)
        {
        }

        public override string Execute(IWordDictionary wordDictionary)
        {
            var result = wordDictionary.Add(Key, Values.ToArray());
            var sb = new StringBuilder();
            sb.Append("(");
            sb.Append(string.Join(",", result));
            sb.Append($"): values of word \"{Key}\" has been successfully added to dictionary.");
            return sb.ToString();
        }

        public override bool IsValid()
        {
            var isBaseValid = base.IsValid();
            var isValuesValid = Values.Length > 0;
            return isBaseValid && isValuesValid;
        }
    }
}