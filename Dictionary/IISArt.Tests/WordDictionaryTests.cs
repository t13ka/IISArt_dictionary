using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IISArt.Tests
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using IISArt.Abstractions;
    using IISArt.Server.NinjectIoc;

    using Ninject;

    [TestClass]
    public class WordDictionaryTests
    {
        private const string Key = "test";

        private readonly StandardKernel _kernel;

        private IWordDictionary _dictionary;

        public WordDictionaryTests()
        {
            var registrations = new NinjectRegistrations();
            _kernel = new StandardKernel(registrations);
        }

        [TestMethod]
        public void AddMethodTests()
        {
            _dictionary = _kernel.Get<IWordDictionary>();

            _dictionary.Add(Key, new[] { "v1" });
            CollectionAssert.Contains(_dictionary.Get(Key).ToList(), "v1");

            _dictionary.Add(Key, new[] { "v2" });
            CollectionAssert.Contains(_dictionary.Get(Key).ToList(), "v2");

            _dictionary.Add(Key, new[] { "v2" });
            Assert.AreEqual(_dictionary.Get(Key).ToList().Count, 2);

            CollectionAssert.DoesNotContain(_dictionary.Get(Key).ToList(), "v3");

            _dictionary.Add(Key, new[] { string.Empty });
            Assert.AreEqual(3, _dictionary.Get(Key).ToList().Count);
        }

        [TestMethod]
        public void DeleteMethodTests()
        {
            _dictionary = _kernel.Get<IWordDictionary>();
            _dictionary.Add(Key, new[] { "v1", "v2", "v3" });
            _dictionary.Delete(Key, new[] { "v1" });
            var coll = _dictionary.Get(Key).ToList();
            CollectionAssert.DoesNotContain(coll, "v1");
        }

        [TestMethod]
        public void DeleteNotInListMethodTests()
        {
            _dictionary = _kernel.Get<IWordDictionary>();
            _dictionary.Delete(Key, new[] { "v1" });
            CollectionAssert.DoesNotContain(_dictionary.Get("v1"), "v1");
        }

        [TestMethod]
        public void GetMethodTests()
        {
            _dictionary = _kernel.Get<IWordDictionary>();

            _dictionary.Add(Key, new[] { "v1", "v2", "v3" });
            var set = _dictionary.Get(Key);
            var collection = set.ToList();
            CollectionAssert.Contains(collection, "v1");
            CollectionAssert.Contains(collection, "v2");
            CollectionAssert.Contains(collection, "v3");
            CollectionAssert.DoesNotContain(collection, "v4");
        }
    }
}