using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ProgramSystem.Bll.Services.Interfaces;
using Repository.factories;
using Services;
using Services.Interfaces;

namespace AutofacDependence
{
    public class ServicesModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new UserService(c.Resolve<ISqlLiteRepositoryContextFactory>()))
                .As<IUserService>();
            builder.Register(c => new ParametersService(c.Resolve<ISqlLiteRepositoryContextFactory>()))
                .As<IParameterService>();
            builder.Register(c => new MethodService(c.Resolve<ISqlLiteRepositoryContextFactory>()))
                .As<IMethodService>();
            builder.Register(c => new TasksService(c.Resolve<ISqlLiteRepositoryContextFactory>()))
                .As<ITasksService>();
            builder.Register(c => new UnitOfMeasService(c.Resolve<ISqlLiteRepositoryContextFactory>()))
                .As<IUnitOfMeasService>();
        }
    }
}
