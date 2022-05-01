using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using ModelsView;
using ProgramSystem.Bll.Services.Interfaces;
using ReactiveUI;

namespace Administrator.ViewModel
{
    public class AbstractUsersViewModel : ReactiveObject
    {
        private readonly IUserService _userService;
        public AbstractUsersViewModel(IUserService userService)
        {
            _userService = userService;
            UsersTable = new ObservableCollection<UserView>(_userService.GetAllUsers());
        }

        #region Fields

        private string login;
        private string password;
        private ObservableCollection<string> roles;
        private string selectedRole;
        private ObservableCollection<UserView> usersTable;
        private UserView selectedUserRow;
        #endregion

        #region Properties

        public string Login
        {
            get { return login;}
            set => this.RaiseAndSetIfChanged(ref login, value);
        }

        public string Password
        {
            get { return password; }
            set => this.RaiseAndSetIfChanged(ref password, value);
        }
        public ObservableCollection<string> Roles
        {
            get { return roles; }
            set => this.RaiseAndSetIfChanged(ref roles, value);
        }

        public string SelectedRole
        {
            get { return selectedRole; }
            set => this.RaiseAndSetIfChanged(ref selectedRole, value);
        }

        public ObservableCollection<UserView> UsersTable
        {
            get { return usersTable; }
            set => this.RaiseAndSetIfChanged(ref usersTable, value);
        }

        public UserView SelectedUserRow
        {
            get { return selectedUserRow; }
            set => this.RaiseAndSetIfChanged(ref selectedUserRow, value);
        }
        #endregion

        #region Command

        

        #endregion
    }
}
