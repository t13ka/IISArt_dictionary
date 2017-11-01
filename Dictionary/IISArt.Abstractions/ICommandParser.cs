namespace IISArt.Abstractions
{
    public interface ICommandParser
    {
        ICommand Parse(string message);

        ICommand Parse(string[] args);
    }
}