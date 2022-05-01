using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramSystem.Bll.Services.Interfaces;
using ReactiveUI;

namespace Administrator.ViewModel
{
    public class AdministrationViewModel :  AbstractUsersViewModel
    {
        public AdministrationViewModel(IUserService userService) : base(userService)
        {
        }
    }
}
