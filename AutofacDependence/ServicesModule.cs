using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ProgramSystem.Bll.Services.Interfaces;
using Repository.factories;
using Services;

namespace AutofacDependence
{
    public class ServicesModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new UserService(c.Resolve<ISqlLiteRepositoryContextFactory>()))
                .As<IUserService>();
        }
    }
}
