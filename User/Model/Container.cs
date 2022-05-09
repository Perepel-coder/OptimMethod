using Autofac;
using System.Collections.ObjectModel;
using User.Model.FileServices;
using User.ViewModel;

namespace User.Model
{
    internal static class Container
    {
        public static ContainerBuilder GetBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UserViewModel>().AsSelf();
            builder.RegisterType<DialogService>().AsSelf().As<IDialogService>();
            builder.RegisterType<FileService>().AsSelf().As<IFileService>();
            builder.Register((c, p) => new Chart3DViewModel(p.Named<ObservableCollection<Point3>>("p1"))).AsSelf();
            builder.Register((c, p) => new TaskDescriptionViewModel(p.Named<string>("p1"))).AsSelf();
            return builder;
        }
    }
}
