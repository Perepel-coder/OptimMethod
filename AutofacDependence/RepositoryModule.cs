using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Repository.factories;

namespace AutofacDependence
{
    public class RepositoryModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new SqlLiteRepositoryContextFactory("Data Source = DatabaseMO.db"))
                .As<ISqlLiteRepositoryContextFactory>();
        }
    }
}
