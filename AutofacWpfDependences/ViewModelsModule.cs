using System;
using AdministratorFormsWPF.ViewModel;
using Autofac;
using ProgramSystem.Bll.Services.Interfaces;
using Services.Interfaces;

namespace AutofacWpfDependences
{
    public class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AdministrationViewModel(
                c.Resolve<IUserService>(),
                c.Resolve<IMethodService>(),
                c.Resolve<IParameterService>(),
                c.Resolve<ITasksService>(),
                c.Resolve<IUnitOfMeasService>()))
                .AsSelf();
        }
    }
}
