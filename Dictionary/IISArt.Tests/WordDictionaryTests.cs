using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IISArt.Tests
{
    using System.Linq;

    using IISArt.Server;

    [TestClass]
    public class WordDictionaryTests
    {
        private const string Key = "test";

        [TestMethod]
        public void AddMethodTests()
        {
            var d = new WordDictionary();
            d.Add(Key, new[] { "v1" });
            CollectionAssert.Contains(d.Get(Key).ToList(), "v1");

            d.Add(Key, new[] { "v2" });
            CollectionAssert.Contains(d.Get(Key).ToList(), "v2");

            d.Add(Key, new[] { "v2" });
            Assert.AreEqual(d.Get(Key).ToList().Count, 2);

            CollectionAssert.DoesNotContain(d.Get(Key).ToList(), "v3");

            d.Add(Key, new[] { string.Empty });
            Assert.AreEqual(d.Get(Key).ToList().Count, 2);
        }

        [TestMethod]
        public void DeleteMethodTests()
        {
            var d = new WordDictionary();
            d.Add(Key, new[] { "v1", "v2", "v3" });
            d.Delete(Key, new[] { "v1" });
            CollectionAssert.DoesNotContain(d.Get(Key).ToList(), "v1");
            Assert.AreEqual(d.Get(Key).ToList().Count, 2);
        }

        [TestMethod]
        public void DeleteNotInListMethodTests()
        {
            var d = new WordDictionary();
            d.Delete(Key, new[] { "v1" });
            CollectionAssert.DoesNotContain(d.Get("v1"), "v1");
        }

        [TestMethod]
        public void GetMethodTests()
        {
            var d = new WordDictionary();
            d.Add(Key, new[] { "v1", "v2", "v3" });
            var set = d.Get(Key);
            var collection = set.ToList();
            CollectionAssert.Contains(collection, "v1");
            CollectionAssert.Contains(collection, "v2");
            CollectionAssert.Contains(collection, "v3");
            CollectionAssert.DoesNotContain(collection, "v4");
        }
    }
}