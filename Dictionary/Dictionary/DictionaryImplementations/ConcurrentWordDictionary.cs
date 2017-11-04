namespace IISArt.Server.DictionaryImplementations
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using IISArt.Abstractions;

    public class ConcurrentWordDictionary : IWordDictionary
    {
        private static readonly ConcurrentDictionary<string, string[]> Instance;

        /// <summary>
        /// Initializes static members of the <see cref="ConcurrentWordDictionary"/> class.
        /// </summary>
        static ConcurrentWordDictionary()
        {
            Instance = new ConcurrentDictionary<string, string[]>();
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
            values = PrepareInputSet(values);
            if (values.Length == 0)
            {
                return values;
            }

            var output = values;

            var updateFunc = new Func<string, string[], string[]>(
                (funcKey, set) =>
                    {
                        output = GetValuesWhichAreNotInTheSet(values, set);
                        set = set.Union(values).ToArray();
                        return set;
                    });

            Instance.AddOrUpdate(key, values, updateFunc);
            return output;
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

            values = PrepareInputSet(values);

            if (Instance.TryGetValue(key, out string[] delWords))
            {
                foreach (var value in values)
                {
                    result.Add(value, delWords.Contains(value));
                }

                var newValue = delWords.Except(values).ToArray();
                if (Instance.TryUpdate(key, newValue, Instance[key]))
                {
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
            if (Instance.TryGetValue(key, out string[] set))
            {
                return set;
            }

            return new string[] { };
        }

        private static string[] GetValuesWhichAreNotInTheSet(string[] values, string[] set)
        {
            var list = new List<string>();
            foreach (var value in values)
            {
                if (set.Contains(value) == false)
                {
                    list.Add(value);
                }
            }

            return list.ToArray();
        }

        private static string[] PrepareInputSet(string[] values)
        {
            return values.Distinct().ToArray();
        }
    }
}