using System.Collections.Generic;

namespace IISArt.Abstractions
{
    public interface IWordDictionary
    {
        string[] Add(string key, string[] values);

        Dictionary<string, bool> Delete(string key, string[] values);

        string[] Get(string key);
    }
}