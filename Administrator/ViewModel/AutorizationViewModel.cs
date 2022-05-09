using System.Net;
using System.Security;
using System.Windows;
using AdministratorFormsWPF.View;
using Autofac;
using ProgramSystem.Bll.Services.Interfaces;
using ReactiveUI;
using Startup.Autofac;
using User.View;

namespace Startup.ViewModel
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
                builderBase.RegisterAllModules();
                var containerBase = builderBase.Build();
                var viewBase = containerBase.Resolve<AdministratorWindow>();

                viewBase.Show();
            }
            else if (user.Role == "user")
            {
                // пользователь
                var builderBase = new ContainerBuilder();
                builderBase.RegisterAllModules();
                var containerBase = builderBase.Build();

                var viewBase = containerBase.Resolve<UserWindow>();
                viewBase.Show();
            }
        }

        private bool CanAuthorization() => !string.IsNullOrEmpty(Login) && Password != null; //проверка

        #endregion

    }
}
