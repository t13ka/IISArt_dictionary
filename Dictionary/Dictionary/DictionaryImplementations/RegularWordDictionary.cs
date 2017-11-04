namespace IISArt.Server.DictionaryImplementations
{
    using System.Collections.Generic;
    using System.Linq;

    using IISArt.Abstractions;

    public class RegularWordDictionary : IWordDictionary
    {
        private static readonly Dictionary<string, HashSet<string>> Instance;

        /// <summary>
        /// Initializes static members of the <see cref="RegularWordDictionary"/> class.
        /// </summary>
        static RegularWordDictionary()
        {
            Instance = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// Add new values for key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public string[] Add(string key, string[] values)
        {
            if (values.Length == 0)
            {
                return values;
            }

            var inputSet = new HashSet<string>(values);
            var outputSet = new HashSet<string>(values);

            lock (Instance)
            {
                if (Instance.TryGetValue(key, out HashSet<string> currentSet))
                {
                    outputSet.ExceptWith(currentSet);
                    currentSet.UnionWith(inputSet);
                }
                else
                {
                    Instance.Add(key, inputSet);
                }
            }

            return outputSet.ToArray();
        }

        /// <summary>
        /// Delete values for key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public Dictionary<string, bool> Delete(string key, string[] values)
        {
            var result = new Dictionary<string, bool>();
            var inputSet = new HashSet<string>(values);
            var outputSet = new HashSet<string>(values);

            lock (Instance)
            {
                if (Instance.TryGetValue(key, out HashSet<string> currentSet))
                {
                    outputSet.IntersectWith(currentSet);
                    currentSet.ExceptWith(inputSet);

                    foreach (var item in outputSet)
                    {
                        result.Add(item, true);
                    }

                    foreach (var item in currentSet)
                    {
                        result.Add(item, false);
                    }
                }
                else
                {
                    Instance.Add(key, inputSet);
                }
            }

            return result;
        }

        /// <summary>
        /// Get values by key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public string[] Get(string key)
        {
            var result = new string[] { };

            lock (Instance)
            {
                if (Instance.TryGetValue(key, out HashSet<string> set))
                {
                    result = set.ToArray();
                }
            }

            return result;
        }
    }
}