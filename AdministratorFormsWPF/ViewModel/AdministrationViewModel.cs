using ProgramSystem.Bll.Services.Interfaces;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ModelsView;
using Services.Interfaces;
using ServicesMVVM;

namespace AdministratorFormsWPF.ViewModel
{
    public class AdministrationViewModel :  ReactiveObject
    {
        private readonly IUserService _userService;
        private readonly IMethodService _methodService;
        private readonly IParameterService _parameterService;
        private readonly ITasksService _tasksService;
        private readonly IUnitOfMeasService _unitOfMeasService;

        public AdministrationViewModel(
            IUserService userService, 
            IMethodService methodService, 
            IParameterService parameterService, 
            ITasksService tasksService,
            IUnitOfMeasService unitOfMeasService)
        {
            _userService = userService;
            _methodService = methodService;
            _parameterService = parameterService;
            _tasksService = tasksService;
            _unitOfMeasService = unitOfMeasService;

            // Пользователь
            UsersTable = new ObservableCollection<UserView>(_userService.GetAllUsers());
            Roles = new ObservableCollection<string>(_userService.GetRolesCollection());
            UpdateUsersListCommand = new RelayCommand(obj => UpdateUsersTable());
            ClearUserPropertiesCommand = new RelayCommand(obj => ClearUserProperties());
            AddUserCommand = new AsyncCommand(AddUserAsync, CanAddUser);
            DeleteUserCommand = new AsyncCommand(DeleteUserAsync, CanDeleteUser);
            EditUserCommand = new AsyncCommand(EditUserAsync, CanEditUser);

            // Параметры
            ParametersTable = new ObservableCollection<ParameterView>(_parameterService.GetAllParameters());
            UnitsOfMeasComboBox = new ObservableCollection<string>(_unitOfMeasService.GetAllNamesUnitOfMeas());
            UpdateParametersCommand = new AsyncCommand(UpdateParametersTableAsync, () => true);
            AddParameterCommand = new AsyncCommand(AddParameterAsync, CanAddParameter);
            DeleteParameterCommand = new AsyncCommand(DeleteParameterAsync, CanDeleteParameter);
            EditParameterCommand = new AsyncCommand(EditParameterAsync, CanEditParameter);
            ClearParameterPropertiesCommand = new RelayCommand(obj => ClearParameterProperties());

            // ед. измерения
            UnitOfMeasTable = new ObservableCollection<UnitOfMeasView>(_unitOfMeasService.GetAllUnitsOfMeas());
            UpdateUnitOfMeasTableCommand = new AsyncCommand(UpdateUnitOfMeasTableAsync, () => true);
            ClearUnitOfMeasCommand = new RelayCommand(obj => ClearUnitOfMeas());
            AddUnitOfMeasCommand = new AsyncCommand(AddUnitOfMeasAsync, CanAddUnitOfMeas);
            DeleteUnitOfMeasCommand = new AsyncCommand(RemoveUnitOfMeasAsync, CanDeleteUnitOfMeas);

            // Методы оптимизации
            OptimizationMethodsTable = new ObservableCollection<OptimizationMethodView>(_methodService.GetAllOptimizationMethods());
            UpdateTableOptimizationMethodCommand = new AsyncCommand(UpdateTableMethodsOptimizationAsync, () => true);
            ClearPropertiesMehodsOptimizationCommand =
                new RelayCommand(obj => ClearProperiesOfOptimizatiomMethods());
            AddOptimizationMethodCommand = new AsyncCommand(AddOptimizationMethodAsync, CanAddOptimizationMethod);
            DeleteOptimizationMethodCommand = new AsyncCommand(DeleteOptimizationMethodAsync, CanDeleteOptimizationMethod);
            EditOptimizationMethodAsyncCommand =
                new AsyncCommand(EditOptimizationMethodAsync, CanEditOptimizationMethod);

            // Задачи
            TasksTable = new ObservableCollection<TaskView>(_tasksService.GetAllTask());
            ParametersForTaskComboBox = new ObservableCollection<ParameterView>(_parameterService.GetAllParameters());
            ParametersByTaskTable = new ObservableCollection<TaskParameterValueView>(_tasksService.GetAllParametersValues());
            TasksComboBox = new ObservableCollection<TaskView>(_tasksService.GetAllTask());
            TasksForViewParametersComboBox = new ObservableCollection<TaskView>(_tasksService.GetAllTask());
            UpdateTasksTableCommand = new AsyncCommand(UpdateTaskTableAsync, () => true);
            UpdateParametersForTaskTableCommand = new AsyncCommand(UpdateParameteresForTaskTableAsync, () => true);
            ClearTaskPropertiesCommand = new RelayCommand(obj => ClearTaskProperties());
            AddTaskCommand = new AsyncCommand(AddTaskAsync, CanAddTask);
            DeleteTaskCommand = new AsyncCommand(DeleteTaskAsync, CanDeleteTask);
            EditTaskCommand = new AsyncCommand(EditTaskAsync, CanEditTask);
            AddParameterForTaskCommand = new AsyncCommand(AddParameterByTaskAsync, CanAddParameterForTask);
            DeleteParameterForTaskCommand = new AsyncCommand(DeleteParameterByTaskAsync,CanDeleteParameterForTask);
            EditParameterForTaskCommand = new AsyncCommand(EditParameterByTaskAsync, CanEditParameterForTask);
        }

        #region Fields

        //Пользователи
        private string? _login;
        private string? _password;
        private ObservableCollection<string>? _roles;
        private string? _selectedRole;
        private ObservableCollection<UserView>? _usersTable;
        private UserView? _selectedUserRow;
        
        // Параметры
        private string? _parameterName;
        private ParameterView? _selectedParameterRow;
        private ObservableCollection<ParameterView>? _parametersTable;
        private ObservableCollection<string>? _unitsOfMeasComboBox;
        private string? _selectedUnitOfMeas;
        private string? _notation;

        // единицы измерения
        private string? _unitOfMeasName;
        private ObservableCollection<UnitOfMeasView> ? _unitOfMeasTable;
        private UnitOfMeasView? _selectedUnitOfMeasRow;

        // методы оптимизации
        private string? _methodOptimizationName;
        private OptimizationMethodView ? _selectedOptimizationMethodRow;
        private ObservableCollection<OptimizationMethodView> ? _optimizationMethodsTable;
        private bool _isRealisedOptimizationMethod;

        // Задачи
        private string? _taskName;
        private string? _taskDescription;
        private TaskView? _selectedTaskRow;
        private ObservableCollection<TaskView>? _tasksTable;

        private ParameterView?  _selectedParameterForTaskComboBox;
        private ObservableCollection<ParameterView>? _parametersForTaskComboBox;
        public TaskView? _selectedTaskComboBox;
        public ObservableCollection<TaskView>? _tasksComboBox;
        private double? _parameterByTaskValue;
        private ObservableCollection<TaskParameterValueView>? _parametersByTaskTable;
        private TaskParameterValueView? _selectedParameterByTaskRow;
        private TaskView? _selectedTaskForViewParameters;
        private ObservableCollection<TaskView>? _tasksForViewParametersComboBox;
        #endregion


        #region Properties

        #region Задачи

        public string? TaskName
        {
            get => _taskName;
            set => this.RaiseAndSetIfChanged(ref _taskName, value);
        }
        public string? TaskDescroption
        {
            get => _taskDescription;
            set => this.RaiseAndSetIfChanged(ref _taskDescription, value);
        }
        public TaskView? SelectedTaskRow
        {
            get => _selectedTaskRow;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedTaskRow, value);
                if (_selectedTaskRow != null)
                {
                    TaskName = _selectedTaskRow.Name;
                    TaskDescroption = _selectedTaskRow.Description;
                }
                else
                {
                    ParametersByTaskTable = new ObservableCollection<TaskParameterValueView>(_tasksService.GetAllParametersValues());
                }
            }
        }
        public ObservableCollection<TaskView>? TasksTable
        {
            get => _tasksTable;
            set => this.RaiseAndSetIfChanged(ref _tasksTable, value);
        }

        public ParameterView?  SelectedParameterForTaskComboBox
        {
            get => _selectedParameterForTaskComboBox;
            set => this.RaiseAndSetIfChanged(ref _selectedParameterForTaskComboBox, value);
        }
        public ObservableCollection<ParameterView>? ParametersForTaskComboBox
        {
            get => _parametersForTaskComboBox;
            set => this.RaiseAndSetIfChanged(ref _parametersForTaskComboBox, value);
        }

        public TaskView? SelectedTaskComboBox
        {
            get => _selectedTaskComboBox;
            set => this.RaiseAndSetIfChanged(ref _selectedTaskComboBox, value);
        }
        public ObservableCollection<TaskView>? TasksComboBox
        {
            get => _tasksComboBox;
            set => this.RaiseAndSetIfChanged(ref _tasksComboBox, value);
        }
        public double? ParameterByTaskValue
        {
            get => _parameterByTaskValue;
            set => this.RaiseAndSetIfChanged(ref _parameterByTaskValue, value);
        }
        public TaskParameterValueView? SelectedParameterByTaskRow
        {
            get => _selectedParameterByTaskRow;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedParameterByTaskRow, value);
                if (_selectedParameterByTaskRow != null)
                {
                    if (ParametersForTaskComboBox != null)
                        SelectedParameterForTaskComboBox = ParametersForTaskComboBox
                            .FirstOrDefault(x => x.Id == _selectedParameterByTaskRow.ParameterId);
                    if (TasksComboBox != null)
                        SelectedTaskComboBox = TasksComboBox
                            .FirstOrDefault(x => x.IdTask == _selectedParameterByTaskRow.TaskId);
                    ParameterByTaskValue = _selectedParameterByTaskRow.Value;
                }
            }
        }
        public ObservableCollection<TaskParameterValueView>? ParametersByTaskTable
        {
            get => _parametersByTaskTable;
            set => this.RaiseAndSetIfChanged(ref _parametersByTaskTable, value);
        }
        public TaskView? SelectedTaskForViewParameters
        {
            get => _selectedTaskForViewParameters;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedTaskForViewParameters, value);
                if (_selectedTaskForViewParameters != null)
                {
                    if (TasksComboBox != null)
                        SelectedTaskComboBox = TasksComboBox
                            .FirstOrDefault(x => x.IdTask == _selectedTaskForViewParameters.IdTask);
                    ParametersByTaskTable =
                        new ObservableCollection<TaskParameterValueView>(_tasksService.GetParametersByTaskId(_selectedTaskForViewParameters.IdTask));
                }
                else
                {
                    ParametersByTaskTable =
                        new ObservableCollection<TaskParameterValueView>(_tasksService.GetAllParametersValues());
                }
            }
        }
        public ObservableCollection<TaskView>? TasksForViewParametersComboBox
        {
            get => _tasksForViewParametersComboBox;
            set => this.RaiseAndSetIfChanged(ref _tasksForViewParametersComboBox, value);
        }
        #endregion
        #region пользователи

        public string? Login
        {
            get { return _login; }
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }
        public string? Password
        {
            get { return _password; }
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        public ObservableCollection<string>? Roles
        {
            get => _roles;
            set => this.RaiseAndSetIfChanged(ref _roles, value);
        }
        public string? SelectedRole
        {
            get => _selectedRole;
            set => this.RaiseAndSetIfChanged(ref _selectedRole, value);
        }
        public ObservableCollection<UserView>? UsersTable
        {
            get => _usersTable;
            set => this.RaiseAndSetIfChanged(ref _usersTable, value);
        }
        public UserView? SelectedUserRow
        {
            get => _selectedUserRow;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedUserRow, value);
                if (_selectedUserRow != null)
                {
                    Login = _selectedUserRow.Login;
                    Password = _selectedUserRow.Password;
                    SelectedRole = _selectedUserRow.Role;
                }
                else
                {
                    Login = null;
                    Password = null;
                    SelectedRole = null;
                }
            }
        }

        #endregion
        #region параметры
        public string? ParameterName
        {
            get => _parameterName;
            set => this.RaiseAndSetIfChanged(ref _parameterName, value);
        }

        public string? Notation
        {
            get => _notation;
            set => this.RaiseAndSetIfChanged(ref _notation, value);
        }
        public ParameterView? SelectedParameterRow
        {
            get => _selectedParameterRow;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedParameterRow, value);
                if (_selectedParameterRow != null)
                {
                    ParameterName = _selectedParameterRow.Description;
                    SelectedUnitOfMeas = _selectedParameterRow.UnitOfMeasName;
                    Notation = _selectedParameterRow.Notation;
                }
                else
                {
                    Notation = null;
                    ParameterName = null;
                    SelectedUnitOfMeas = null;
                }
            }
        }
        public ObservableCollection<ParameterView>? ParametersTable
        {
            get => _parametersTable;
            set => this.RaiseAndSetIfChanged(ref _parametersTable, value);
        }
        public ObservableCollection<string>? UnitsOfMeasComboBox
        {
            get => _unitsOfMeasComboBox;
            set => this.RaiseAndSetIfChanged(ref _unitsOfMeasComboBox, value);
        }
        public string? SelectedUnitOfMeas
        {
            get => _selectedUnitOfMeas;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedUnitOfMeas, value);
            }
        }


        #endregion
        #region едизмерения

        public string? UnitOfMeasName
        {
            get => _unitOfMeasName;
            set => this.RaiseAndSetIfChanged(ref _unitOfMeasName, value);
        }

        public ObservableCollection<UnitOfMeasView>? UnitOfMeasTable
        {
            get => _unitOfMeasTable;
            set => this.RaiseAndSetIfChanged(ref _unitOfMeasTable, value);
        }

        public UnitOfMeasView? SelectedUnitOfMeasRow
        {
            get => _selectedUnitOfMeasRow;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedUnitOfMeasRow, value);
                if (_selectedUnitOfMeasRow != null)
                    UnitOfMeasName = _selectedUnitOfMeasRow.Name;
            }
        }

        #endregion
        #region методы_оптимизации

        public string? MethodOptimizationName
        {
            get => _methodOptimizationName;
            set => this.RaiseAndSetIfChanged(ref _methodOptimizationName, value);
        }
        public OptimizationMethodView? SelectedOptimizationMethodRow
        {
            get => _selectedOptimizationMethodRow;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedOptimizationMethodRow, value);
                if (_selectedOptimizationMethodRow != null)
                {
                    MethodOptimizationName = _selectedOptimizationMethodRow.Name;
                    IsRealisedOptimizationMethod = _selectedOptimizationMethodRow.IsRealized;
                }
            }
        }
        public ObservableCollection<OptimizationMethodView>? OptimizationMethodsTable
        {
            get => _optimizationMethodsTable;
            set => this.RaiseAndSetIfChanged(ref _optimizationMethodsTable, value);
        }
        public bool IsRealisedOptimizationMethod
        {
            get => _isRealisedOptimizationMethod;
            set => this.RaiseAndSetIfChanged(ref _isRealisedOptimizationMethod, value);

        }
        #endregion

        #endregion


        #region Command

        #region Команды_задачи

        public AsyncCommand UpdateTasksTableCommand { get; set; }
        public AsyncCommand UpdateParametersForTaskTableCommand { get; set; }
        public RelayCommand ClearTaskPropertiesCommand { get; set; }

        public AsyncCommand AddTaskCommand { get; set; }
        public AsyncCommand DeleteTaskCommand { get; set; }
        public AsyncCommand EditTaskCommand { get; set; }

        public AsyncCommand AddParameterForTaskCommand { get; set; }
        public AsyncCommand DeleteParameterForTaskCommand { get; set; }
        public AsyncCommand EditParameterForTaskCommand { get; set; }

        #endregion
        #region Команды_Пользователей
        public RelayCommand UpdateUsersListCommand { get; set; }
        public RelayCommand ClearUserPropertiesCommand { get; set; }
        public AsyncCommand AddUserCommand { get; set; }
        public AsyncCommand DeleteUserCommand { get; set; }
        public AsyncCommand EditUserCommand { get; set; }
        #endregion
        #region Команды_Параметров
        // Параметры
        public AsyncCommand UpdateParametersCommand { get; set; }
        public AsyncCommand AddParameterCommand { get; set; }
        public AsyncCommand DeleteParameterCommand { get; set; }
        public AsyncCommand EditParameterCommand { get; set; }
        public RelayCommand ClearParameterPropertiesCommand { get; set; }

        #endregion
        #region Команды_Ед_измерений
        // ед. измерения
        public RelayCommand ClearUnitOfMeasCommand { get; set; }
        public AsyncCommand AddUnitOfMeasCommand { get; set; }
        public AsyncCommand DeleteUnitOfMeasCommand { get; set; }
        public AsyncCommand UpdateUnitOfMeasTableCommand { get; set; }
        #endregion
        #region Команды_Методы_оптимизации
        public AsyncCommand UpdateTableOptimizationMethodCommand { get; set; }
        public RelayCommand ClearPropertiesMehodsOptimizationCommand { get; set; }
        public AsyncCommand AddOptimizationMethodCommand { get; set; }
        public AsyncCommand DeleteOptimizationMethodCommand { get; set; }
        public AsyncCommand EditOptimizationMethodAsyncCommand { get; set; }
        #endregion

        #endregion


        #region CanMethod

        #region Can_задачи

        private bool CanAddTask() => !string.IsNullOrEmpty(TaskName) 
                                     && !string.IsNullOrEmpty(TaskDescroption);
        private bool CanDeleteTask() => SelectedTaskRow != null;
        private bool CanEditTask() => SelectedTaskRow != null 
                                      && !string.IsNullOrEmpty(TaskName)
                                      && !string.IsNullOrEmpty(TaskDescroption);

        private bool CanAddParameterForTask() => SelectedTaskComboBox != null 
                                                 && ParameterByTaskValue != null
                                                 && SelectedParameterForTaskComboBox != null;
        private bool CanDeleteParameterForTask() => SelectedParameterByTaskRow != null;
        private bool CanEditParameterForTask() => SelectedTaskComboBox != null 
                                                  && SelectedParameterByTaskRow != null
                                                  && ParameterByTaskValue != null
                                                  && SelectedParameterForTaskComboBox != null;

        #endregion
        #region Can_пользователи
        // Пользователь
        private bool CanAddUser() => !string.IsNullOrEmpty(Login)
                                     && !string.IsNullOrEmpty(Password)
                                     && !string.IsNullOrEmpty(SelectedRole);
        private bool CanDeleteUser() => SelectedUserRow != null;
        private bool CanEditUser() => SelectedUserRow != null
                                      && !string.IsNullOrEmpty(Login)
                                      && !string.IsNullOrEmpty(Password)
                                      && !string.IsNullOrEmpty(SelectedRole);
        #endregion
        #region Can_Параметры
        // Параметры
        private bool CanAddParameter() => !string.IsNullOrEmpty(ParameterName)
                                          && !string.IsNullOrEmpty(SelectedUnitOfMeas)
                                          && !string.IsNullOrEmpty(Notation);
        private bool CanDeleteParameter() => SelectedParameterRow != null;
        private bool CanEditParameter() => !string.IsNullOrEmpty(ParameterName)
                                           && !string.IsNullOrEmpty(SelectedUnitOfMeas)
                                           && SelectedParameterRow != null
                                           && !string.IsNullOrEmpty(Notation);

        #endregion
        #region Can_ЕдИзмерения

        private bool CanAddUnitOfMeas() => !string.IsNullOrEmpty(UnitOfMeasName);
        private bool CanDeleteUnitOfMeas() => SelectedUnitOfMeasRow != null;

        #endregion
        #region Can_Методы_оптимизации

        private bool CanAddOptimizationMethod() => !string.IsNullOrEmpty(MethodOptimizationName);
        private bool CanDeleteOptimizationMethod() => SelectedOptimizationMethodRow != null;

        private bool CanEditOptimizationMethod() => SelectedOptimizationMethodRow != null &&
                                                    !string.IsNullOrEmpty(MethodOptimizationName);
        #endregion

        #endregion


        #region Methods

        #region Методы_задачи

        private async Task UpdateTaskTableAsync()
        {
            TasksTable = new ObservableCollection<TaskView>(await _tasksService.GetAllTaskAsync());
            TasksComboBox = new ObservableCollection<TaskView>(await _tasksService.GetAllTaskAsync());
            TasksForViewParametersComboBox = new ObservableCollection<TaskView>(await _tasksService.GetAllTaskAsync());
        }
        private async Task UpdateParameteresForTaskTableAsync()
        {
            if (SelectedTaskForViewParameters != null)
            {
                ParametersByTaskTable = new ObservableCollection<TaskParameterValueView>(await _tasksService.GetParametersByTaskIdAsync(SelectedTaskForViewParameters.IdTask));
            }
            else
            {
                ParametersByTaskTable = new ObservableCollection<TaskParameterValueView>(await _tasksService.GetAllParametersAsync());
            }
            ParametersForTaskComboBox = new ObservableCollection<ParameterView>(await _parameterService.GetAllParametersAsync());
        }

        private void ClearTaskProperties()
        {
            TaskName = null;
            TaskDescroption = null;
            SelectedTaskRow = null;
            SelectedParameterForTaskComboBox = null;
            ParameterByTaskValue = null;
            SelectedParameterByTaskRow = null;
            SelectedTaskComboBox = null;
            SelectedTaskForViewParameters = null;
        }
        private async Task AddTaskAsync()
        {
            if(string.IsNullOrEmpty(TaskName) && string.IsNullOrEmpty(TaskDescroption))
                return;
            try
            {
                await _tasksService.AddTaskAsync(TaskName, TaskDescroption);
            }
            catch
            {
                MessageBox.Show("Такая задача уже есть");
            }
            TasksTable = new ObservableCollection<TaskView>(await _tasksService.GetAllTaskAsync());
        }
        private async Task DeleteTaskAsync()
        {
            if(SelectedTaskRow == null)
                return;
            await _tasksService.DeleteTaskAsync(SelectedTaskRow.IdTask);
        }
        private async Task EditTaskAsync()
        {
            if (!CanEditTask()) return;
            await _tasksService.EditTaskAsync(SelectedTaskRow.IdTask, TaskName, TaskDescroption);
        }
        private async Task AddParameterByTaskAsync()
        {
            if(!CanAddParameterForTask()) return;
            if (SelectedParameterForTaskComboBox != null && SelectedTaskComboBox != null)
            {
                try
                {
                    await _tasksService.AddParameterTaskAsync(SelectedParameterForTaskComboBox.Id ?? 0,
                        SelectedTaskComboBox.IdTask, ParameterByTaskValue ?? 0);
                }
                catch
                {
                    MessageBox.Show("Такой параметр уже добавлен");
                }
            }
        }
        private async Task DeleteParameterByTaskAsync()
        {
            if(!CanDeleteParameterForTask())
                return;
            if (SelectedParameterByTaskRow != null)
                await _tasksService.DeleteParameterTaskAsync(SelectedParameterByTaskRow.ParameterId,
                    SelectedParameterByTaskRow.TaskId);
        }
        private async Task EditParameterByTaskAsync()
        {
            if (!CanEditParameterForTask())
                return;
            if (SelectedParameterByTaskRow != null)
                await _tasksService.EditParameterTaskAsync(SelectedParameterByTaskRow.ParameterId,
                    SelectedParameterByTaskRow.TaskId, ParameterByTaskValue ?? 0);
        }
        #endregion
        #region Методы_пользователей
        private void UpdateUsersTable()
        {
            UsersTable = new ObservableCollection<UserView>(_userService.GetAllUsers());
            Roles = new ObservableCollection<string>(_userService.GetRolesCollection());
        }
        private void ClearUserProperties()
        {
            SelectedRole = null;
            Login = null;
            Password = null;
            SelectedUserRow = null;
        }
        public async Task AddUserAsync()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(SelectedRole))
            {
                return;
            }

            if (_userService.UserIsYetRegister(Login))
            {
                MessageBox.Show("Такой пользователь уже существует!");
                return;
            }
            await _userService.AddUserAsync(new UserView()
            {
                Login = this.Login,
                Password = this.Password,
                Role = this.SelectedRole
            });
            UpdateUsersTable();
        }
        public async Task DeleteUserAsync()
        {
            if (SelectedUserRow == null || SelectedUserRow.Id == null)
            {
                return;
            }

            int id = SelectedUserRow.Id ?? -1;
            if (id == -1)
                return;
            await _userService.RemoveRangeAsync(new[] { id });
            UpdateUsersTable();
        }
        public async Task EditUserAsync()
        {
            if (SelectedUserRow == null
                || SelectedUserRow.Id == null
                || string.IsNullOrEmpty(Login)
                || string.IsNullOrEmpty(Password)
                || SelectedRole == null)
            {
                return;
            }

            await _userService.UpdateUserAsync(new UserView()
            {
                Id = SelectedUserRow.Id,
                Login = this.Login,
                Password = this.Password,
                Role = this.SelectedRole
            });

            UpdateUsersTable();
        }


        #endregion
        #region Методы_параметров
        // Парамерты
        private async Task UpdateParametersTableAsync()
        {
            ParametersTable = new ObservableCollection<ParameterView>(await _parameterService.GetAllParametersAsync());
            UnitsOfMeasComboBox = new ObservableCollection<string>(await _unitOfMeasService.GetAllNamesUnitOfMeasAsync());
        }
        private async Task AddParameterAsync()
        {
            if (string.IsNullOrEmpty(ParameterName) || string.IsNullOrEmpty(SelectedUnitOfMeas) || string.IsNullOrEmpty(Notation))
                return;

            await _parameterService.AddParameterAsync(new ParameterView()
            {
                Description = ParameterName,
                UnitOfMeasName = SelectedUnitOfMeas,
                Notation = Notation
            });

            await UpdateParametersTableAsync();
        }
        private async Task DeleteParameterAsync()
        {
            if (SelectedParameterRow == null || SelectedParameterRow.Id == null)
                return;
            int id = SelectedParameterRow.Id ?? -1;
            if (id == -1)
                return;

            await _parameterService.DeleteParameterAsync(id);
            await UpdateParametersTableAsync();
        }
        private async Task EditParameterAsync()
        {
            if (SelectedParameterRow == null || SelectedUnitOfMeas == null
                                             || string.IsNullOrEmpty(ParameterName))
                return;

            ParameterView parameterView = SelectedParameterRow;
            parameterView.Description = ParameterName;
            parameterView.UnitOfMeasName = SelectedUnitOfMeas;
            parameterView.Notation = Notation;

            await _parameterService.EditParameterAsync(parameterView);
            await UpdateParametersTableAsync();
        }
        private void ClearParameterProperties()
        {
            SelectedParameterRow = null;
            ParameterName = null;
            SelectedUnitOfMeas = null;
            Notation = null;
        }



        #endregion
        #region Методы_ЕдИзмерения

        private void ClearUnitOfMeas()
        {
            UnitOfMeasName = null;
            SelectedUnitOfMeasRow = null;
        }

        private async Task AddUnitOfMeasAsync()
        {
            await _unitOfMeasService.AddUnitOfMeasAsync(UnitOfMeasName);
            await UpdateUnitOfMeasTableAsync();
        }

        private async Task RemoveUnitOfMeasAsync()
        {
            if(SelectedUnitOfMeasRow == null || SelectedUnitOfMeasRow.Id == null)
                return;
            await _unitOfMeasService.RemoveUnitOfMeasAsync(SelectedUnitOfMeasRow.Id);
            await UpdateUnitOfMeasTableAsync();
        }

        private async Task UpdateUnitOfMeasTableAsync()
        {
            UnitOfMeasTable = new ObservableCollection<UnitOfMeasView>(await _unitOfMeasService.GetAllUnitOfMeasAsync());
        }
        #endregion
        #region Методы_Методы_оптимизации

        private async Task UpdateTableMethodsOptimizationAsync()
        {
            OptimizationMethodsTable =
                new ObservableCollection<OptimizationMethodView>(await _methodService.GetAllOptimizationMethodsAsync());
        }

        private void ClearProperiesOfOptimizatiomMethods()
        {
            SelectedOptimizationMethodRow = null;
            MethodOptimizationName = null;
        }
        private async Task AddOptimizationMethodAsync()
        {
            if(!string.IsNullOrEmpty( MethodOptimizationName)) 
                await _methodService.AddOptimizationMethodAsync(MethodOptimizationName, IsRealisedOptimizationMethod);
            await UpdateTableMethodsOptimizationAsync();
        }

        private async Task DeleteOptimizationMethodAsync()
        {
            if (SelectedOptimizationMethodRow != null)
                await _methodService.DeleteOptimizationMethodAsync(SelectedOptimizationMethodRow.Id);
            await UpdateTableMethodsOptimizationAsync();
        }

        private async Task EditOptimizationMethodAsync()
        {
            if (!string.IsNullOrEmpty(MethodOptimizationName) && SelectedOptimizationMethodRow != null)
                await _methodService.EditOptimizationMethod(SelectedOptimizationMethodRow.Id, MethodOptimizationName, IsRealisedOptimizationMethod);
            await UpdateTableMethodsOptimizationAsync();
        }
        #endregion

        #endregion
    }
}
