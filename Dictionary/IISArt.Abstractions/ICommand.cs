namespace IISArt.Abstractions
{
    public interface ICommand
    {
        string Execute(IWordDictionary wordDictionary);

        bool IsValid();
    }
}