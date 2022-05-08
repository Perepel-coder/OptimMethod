using Autofac;
using AutofacDependence;
using AutofacWpfDependences;

namespace Startup.Autofac
{
    static class IocRegister
    {
        public static void RegisterAllModules(this ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new ViewModelsModule());
            builder.RegisterModule(new WindowsModule());
        }
    }
}
