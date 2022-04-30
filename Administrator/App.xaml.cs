using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Administrator.View;
using Administrator.ViewModel;
using Autofac;
using AutofacDependence;
using ProgramSystem.Bll.Services.Interfaces;

namespace Administrator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var builderBase = new ContainerBuilder();

            builderBase.RegisterModule(new RepositoryModule());
            builderBase.RegisterModule(new ServicesModule());

            var containerBase = builderBase.Build();

            var viewmodelBase = new AutorizationViewModel(containerBase.Resolve<IUserService>());

            var viewBase = new AutorizationWindow { DataContext = viewmodelBase };

            viewBase.Show();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Ошибка\n" + e.Exception.StackTrace + " " + "Исключение: "
                            + e.Exception.GetType().ToString() + " " + e.Exception.Message);

            e.Handled = true;
        }
    }
}
