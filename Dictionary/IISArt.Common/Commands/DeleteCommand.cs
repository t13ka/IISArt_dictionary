namespace IISArt.Common.Commands
{
    using System;
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

            if (result.Count == 0)
            {
                return $"Word:\"{Key}\" is not exists in dictionary.{Environment.NewLine}";
            }

            var sb = new StringBuilder();
            foreach (var item in result)
            {
                if (item.Value == false)
                {
                    sb.Append($"Word:\"{Key}\" value:\"{item.Key}\" is not exists in dictionary.{Environment.NewLine}");
                }
                else
                {
                    return $"Values of Word \"{Key}\" has been successfully deleted.";
                }
            }

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