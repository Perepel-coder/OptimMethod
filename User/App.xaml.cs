using Autofac;
using System.Windows;
using User.Model;
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
            var view = new MainWindow { DataContext = container.Resolve<UserViewModel>() };
            view.Show();
        }
    }
   
}
