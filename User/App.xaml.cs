﻿using Autofac;
using System.Windows;
using User.Model;
using User.View;
using User.ViewModel;

namespace User
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void _Startup(object sender, StartupEventArgs e)
        {
            var container = Container.GetBuilder().Build();
            var view = new UserWindow { DataContext = container.Resolve<UserViewModel>() };
            view.Show();
        }
    }
   
}
