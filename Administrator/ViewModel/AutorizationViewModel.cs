using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using AutofacDependence;
using ProgramSystem.Bll.Services.Interfaces;
using ServicesMVVM;

namespace Administrator.ViewModel
{
    public class AutorizationViewModel: ReactiveObject
    {
        private readonly IUserService _userService;

        #region Fields
        private string? _login;
        private SecureString? _password;
        #endregion

        #region Properties

        public string? Login
        {
            get
            {
                return _login;
            }
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }
        public SecureString? Password
        {
            get
            {
                return _password;
            }
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        #endregion

        #region Commands 
        public ServicesMVVM.RelayCommand AuthorizationCommand { get; set; }
        #endregion

        public AutorizationViewModel(IUserService userService)
        {
            Login = "user/admin";
            //Password = "researcher";
            _userService = userService;

            AuthorizationCommand = new ServicesMVVM.RelayCommand(obj => Authorization(), obj => CanAuthorization());
        }

        #region Methods
        private void Authorization() //запускается при клике на кнопку
        {
            var user = _userService.GetAccountByLoginPassword(Login ?? "", new NetworkCredential("", Password).Password);
            var users = _userService.GetAllUsers();

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            else if (user.Role == "admin")
            {
                var builderBase = new ContainerBuilder();
                builderBase.RegisterModule(new RepositoryModule());
                builderBase.RegisterModule(new ServicesModule());
                var containerBase = builderBase.Build();

                MessageBox.Show("Вход под админом\n Пользователь " + user.Login);
                //var viewmodelBase = new WindowEditViewModel();
                //var viewBase = new WindowEdit { DataContext = viewmodelBase };

                //viewBase.Show();
            }
            else if (user.Role == "user")
            {
                var builderBase = new ContainerBuilder();
                builderBase.RegisterModule(new RepositoryModule());
                builderBase.RegisterModule(new ServicesModule());
                var containerBase = builderBase.Build();

                MessageBox.Show("Вход под исследователем\n Пользователь " + user.Login);

                //var viewmodelBase = new MainWindowProgramViewModel(containerBase.Resolve<IMathService>());
                //var viewBase = new MainWindowProgram { DataContext = viewmodelBase };

                //viewBase.Show();
            }
        }

        private bool CanAuthorization() => !string.IsNullOrEmpty(Login) && Password != null; //проверка

        #endregion

    }
}
