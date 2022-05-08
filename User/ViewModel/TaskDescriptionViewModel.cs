using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.ViewModel
{
    internal class TaskDescriptionViewModel: ReactiveObject
    {
        private string? description;
        public string? Getdescription
        {
            get { return description; }
            set { this.RaiseAndSetIfChanged(ref description, value); }
        }
        public TaskDescriptionViewModel(string description)
        {
            Getdescription = description;
        }
    }
}
