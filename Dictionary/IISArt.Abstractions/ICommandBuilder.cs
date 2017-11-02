namespace IISArt.Abstractions
{
    public interface ICommandBuilder
    {
        ICommand Build(string message);

        ICommand Build(string[] args);
    }
}