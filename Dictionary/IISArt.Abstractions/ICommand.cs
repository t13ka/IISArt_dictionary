namespace IISArt.Abstractions
{
    public interface ICommand
    {
        string Execute(IWordDictionary wordDictionary);

        void Build(string[] args);

        bool IsValid();
    }
}