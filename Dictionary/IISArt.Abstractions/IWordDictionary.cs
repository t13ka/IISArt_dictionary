using System.Collections.Generic;

namespace IISArt.Abstractions
{
    using System;

    public interface IWordDictionary
    {
        string[] Add(string key, string[] values);

        string[] Delete(string key, string[] values);

        string[] DeleteKey(string key);

        string[] Get(string key);
    }
}