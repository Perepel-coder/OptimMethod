using Autofac;
using System.Collections.ObjectModel;
using User.ViewModel;

namespace User.Model
{
    internal static class Container
    {
        public static ContainerBuilder GetBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ComplexBoxingMethod>().AsSelf();
            builder.RegisterType<UserViewModel>().AsSelf();
            builder.RegisterType<Chart3DViewModel>().AsSelf();
            builder.Register((c, p) => new Chart3DViewModel(p.Named<ObservableCollection<Point3>>("p1"))).AsSelf();
            return builder;
        }
    }
}
