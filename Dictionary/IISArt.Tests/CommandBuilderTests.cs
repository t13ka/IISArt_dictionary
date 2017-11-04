using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IISArt.Tests
{
    using IISArt.Abstractions;
    using IISArt.Common.Commands;
    using IISArt.Server.NinjectIoc;

    using Ninject;

    [TestClass]
    public class CommandBuilderTests
    {
        private readonly StandardKernel _kernel;

        public CommandBuilderTests()
        {
            var registrations = new NinjectRegistrations();
            _kernel = new StandardKernel(registrations);
        }

        [TestMethod]
        public void BuildCommandsTests()
        {
            var commandBuilder = _kernel.Get<ICommandBuilder>();

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
            var commandBuilder = _kernel.Get<ICommandBuilder>();

            var action = new Action(() => { commandBuilder.Build("a dd12 he llo ad ll12o p321rivet zdravstvuite"); });
            Assert.ThrowsException<ArgumentNullException>(action);
        }
    }
}