namespace IISArt.Common.Commands
{
    using System.Text;

    using IISArt.Abstractions;

    public class DeleteCommand : BaseCommand
    {
        public DeleteCommand(string[] args)
            : base(args)
        {
        }

        public override string Execute(IWordDictionary wordDictionary)
        {
            var result = wordDictionary.Delete(Key, Values);
            var sb = new StringBuilder();
            sb.Append("(");
            sb.Append(string.Join(",", result));
            sb.Append($"): values of word \"{Key}\" has been successfully deleted from dictionary.");
            return sb.ToString();
        }
    }
}