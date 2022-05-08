using AdministratorFormsWPF.View;
using AdministratorFormsWPF.ViewModel;
using Autofac;

namespace AutofacWpfDependences;

public class WindowsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => new AdministratorWindow() {DataContext = c.Resolve<AdministrationViewModel>()})
            .AsSelf();
    }
}