using AdministratorFormsWPF.View;
using AdministratorFormsWPF.ViewModel;
using Autofac;
using User.View;
using User.ViewModel;

namespace AutofacWpfDependences;

public class WindowsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => new AdministratorWindow() {DataContext = c.Resolve<AdministrationViewModel>()})
            .AsSelf();
        builder.Register(c => new UserWindow() { DataContext = c.Resolve<UserViewModel>() })
            .AsSelf();
    }
}