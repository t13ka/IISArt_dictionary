namespace IISArt.Server.NinjectIoc
{
    using IISArt.Abstractions;
    using IISArt.Common.Commands;
    using IISArt.Server.DictionaryImplementations;

    using Ninject.Modules;

    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IWordDictionary>().To<RegularWordDictionary>().InSingletonScope();
            Bind<ICommandBuilder>().To<OptimizedCommandBuilder>().InSingletonScope();
            Bind<ILogger>().To<Logger>().InSingletonScope();
        }
    }
}