using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IISArt.Tests
{
    using IISArt.Common.Commands;

    [TestClass]
    public class CommandBuilterTests
    {
        [TestMethod]
        public void BuildCommandsTests()
        {
            var commandBuilder = new CommandBuilder();

            var addCommand = commandBuilder.Build("add hello allo privet zdravstvuite");
            Assert.IsInstanceOfType(addCommand, typeof(AddCommand));

            var getCommand = commandBuilder.Build("get hello");
            Assert.IsInstanceOfType(getCommand, typeof(GetCommand));

            var deleteCommand = commandBuilder.Build("delete hello privet");
            Assert.IsInstanceOfType(deleteCommand, typeof(DeleteCommand));
        }

        [TestMethod]
        public void BuildCommandsExceptionsTests()
        {
            var commandBuilder = new CommandBuilder();
            var action = new Action(() => { commandBuilder.Build("a dd12 he llo ad ll12o p321rivet zdravstvuite"); });
            Assert.ThrowsException<ArgumentNullException>(action);
        }
    }
}