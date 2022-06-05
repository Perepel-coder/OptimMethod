using Autofac;
using ModelsView;
using ReactiveUI;
using Services.Interfaces;
using ServicesMVVM;
using System.Collections.ObjectModel;
using System.Linq;
using User.View;
using System.Windows.Input;
using User.Model;
using Xceed.Wpf.Toolkit;
using System.Collections.Generic;
using User.Model.FileServices;
using Syncfusion.XlsIO;

namespace User.ViewModel
{
    public class UserViewModel : ReactiveObject
    {
        private readonly ITasksService _taskService;
        private readonly IMethodService _methodService;

        #region поля
        private ObservableCollection<string> listSing;
        private ObservableCollection<string> listExtremum;
        private ObservableCollection<PointOfFunction> functionValue;
        private ObservableCollection<Point3> chart3Ddata;
        private ObservableCollection<Point3> chart2Ddata;
        private ObservableCollection<TaskView> tasks;
        private ObservableCollection<OptimizationMethodView> methods;
        private ObservableCollection<TaskParameterValueView> taskParameters;
        private OptimizationMethodView currentMethod;
        private List<List<Point3>> exceldata;
        private TaskView currentTask;
        private DialogService dialogService;
        private FileService fileService;
        private VariableFunctionLimitations variableFunctionLimitations;
        private string extremum;
        private string result;
        private int rnd;
        private string formalizedDescriptionOfFunctionPicture;
        #endregion

        #region get; set
        public double Getk
        {
            get { return variableFunctionLimitations.k; }
            set
            {
                var k = variableFunctionLimitations.k;
                this.RaiseAndSetIfChanged(ref k, value);
                variableFunctionLimitations.k = k;
            }
        }
        public double Getb
        {
            get { return variableFunctionLimitations.b; }
            set 
            {
                var b = variableFunctionLimitations.b;
                this.RaiseAndSetIfChanged(ref b, value);
                variableFunctionLimitations.b =  b;
            }
        }
        public double Getxmin
        {
            get { return variableFunctionLimitations.Xmin; }
            set 
            {
                var xmin = variableFunctionLimitations.Xmin;
                this.RaiseAndSetIfChanged(ref xmin, value);
                variableFunctionLimitations.Xmin = xmin;
            }
        }
        public double Getxmax
        {
            get { return variableFunctionLimitations.Xmax; }
            set
            {
                var xmax = variableFunctionLimitations.Xmax;
                this.RaiseAndSetIfChanged(ref xmax, value);
                variableFunctionLimitations.Xmax = xmax;
            }
        }
        public double Getymin
        {
            get { return variableFunctionLimitations.Ymin; }
            set
            {
                var ymin = variableFunctionLimitations.Ymin;
                this.RaiseAndSetIfChanged(ref ymin, value);
                variableFunctionLimitations.Ymin = ymin;
            }
        }
        public double Getymax
        {
            get { return variableFunctionLimitations.Ymax; }
            set
            {
                var ymax = variableFunctionLimitations.Ymax;
                this.RaiseAndSetIfChanged(ref ymax, value);
                variableFunctionLimitations.Ymax = ymax;
            }
        }
        public string Getsing
        {
            get { return variableFunctionLimitations.Sing; }
            set
            {
                var sing = variableFunctionLimitations.Sing;
                this.RaiseAndSetIfChanged(ref sing, value);
                variableFunctionLimitations.Sing = sing;
            }
        }
        public double Getε
        {
            get { return variableFunctionLimitations.ε; }
            set
            {
                var ε = variableFunctionLimitations.ε;
                this.RaiseAndSetIfChanged(ref ε, value);
                variableFunctionLimitations.ε = ε;
            }
        }
        public double Getstep
        {
            get { return variableFunctionLimitations.step; }
            set
            {
                var step = variableFunctionLimitations.step;
                this.RaiseAndSetIfChanged(ref step, value);
                variableFunctionLimitations.step = step;
            }
        }
        public double GetstartX
        {
            get { return variableFunctionLimitations.StartX; }
            set
            {
                var startX = variableFunctionLimitations.StartX;
                this.RaiseAndSetIfChanged(ref startX, value);
                variableFunctionLimitations.StartX = startX;
            }
        }
        public double GetstartY
        {
            get { return variableFunctionLimitations.StartY; }
            set
            {
                var startY = variableFunctionLimitations.StartY;
                this.RaiseAndSetIfChanged(ref startY, value);
                variableFunctionLimitations.StartY = startY;
            }
        }
        public int GetcountComplex
        {
            get { return variableFunctionLimitations.countComplex; }
            set
            {
                var countComplex = variableFunctionLimitations.countComplex;
                this.RaiseAndSetIfChanged(ref countComplex, value);
                variableFunctionLimitations.countComplex = countComplex;
            }
        }
        public string Getextremum
        {
            get { return extremum; }
            set { this.RaiseAndSetIfChanged(ref extremum, value); }
        }
        public string Getresult
        {
            get { return result; }
            set { this.RaiseAndSetIfChanged(ref result, value); }
        }
        public int Getrnd
        {
            get { return rnd; }
            set
            {
                this.RaiseAndSetIfChanged(ref rnd, value);
                if (GetfunctionValue.Count != 0) 
                { 
                    var res = GetfunctionValue[0].Round(rnd);
                    Getresult = 
                        $"Значение целевой функции в точке\n" +
                        $"X = {res.X}\n" +
                        $"Y = {res.Y}\n" +
                        $"Z(X, Y) = {res.FunctionValue}";
                }
            }
        }
        public ObservableCollection<TaskParameterValueView> GettaskParameters
        {
            get { return taskParameters; }
            set { this.RaiseAndSetIfChanged(ref taskParameters, value); }
        }
        public ObservableCollection<string> GetlistSing
        {
            get { return listSing; }
            set { this.RaiseAndSetIfChanged(ref listSing, value); }
        }
        public ObservableCollection<string> GetlistExtremum
        {
            get { return listExtremum; }
            set { this.RaiseAndSetIfChanged(ref listExtremum, value); }
        }
        public ObservableCollection<Point3> Getchart3Ddata
        {
            get { return chart3Ddata; }
            set { this.RaiseAndSetIfChanged(ref chart3Ddata, value); }
        }
        public ObservableCollection<Point3> Getchart2Ddata
        {
            get { return chart2Ddata; }
            set { this.RaiseAndSetIfChanged(ref chart2Ddata, value); }
        }
        public ObservableCollection<PointOfFunction> GetfunctionValue
        {
            get { return functionValue; }
            set { this.RaiseAndSetIfChanged(ref functionValue, value); }
        }
        public ObservableCollection<TaskView> Gettasks
        {
            get { return tasks; }
            set { this.RaiseAndSetIfChanged(ref tasks, value); }
        }
        public ObservableCollection<OptimizationMethodView> Getmethods
        {
            get { return methods; }
            set { this.RaiseAndSetIfChanged(ref methods, value); }
        }
        public OptimizationMethodView GetcurrentMethod
        {
            get { return currentMethod; }
            set { this.RaiseAndSetIfChanged(ref currentMethod, value); }
        }
        public TaskView GetcurrentTask
        {
            get { return currentTask; }
            set 
            {
                this.RaiseAndSetIfChanged(ref currentTask, value);
                GettaskParameters = new(_taskService
                    .GetAllParametersValues()
                    .Where(x => x.TaskId == currentTask.IdTask)
                    .Select(el => el)
                    .ToList());
                GetDescriptionPicture = string.Join("", currentTask.Name.Split(" ")) + ".png";
            }
        }
        public string GetDescriptionPicture
        {
            get { return formalizedDescriptionOfFunctionPicture; }
            set { this.RaiseAndSetIfChanged(ref formalizedDescriptionOfFunctionPicture, value); }
        }
        #endregion

        #region конструкторы
        public UserViewModel(ITasksService tasksService, IMethodService methodService)
        {
            variableFunctionLimitations = new();
            GetlistSing = new() { "⩾", "⩽" };
            GetlistExtremum = new() { "условный максимум", "условный минимум" };
            Getextremum = "условный минимум";
            variableFunctionLimitations.Sing = "⩽";
            variableFunctionLimitations.k = -1;
            variableFunctionLimitations.b = 3;
            variableFunctionLimitations.Xmin = -3;
            variableFunctionLimitations.Xmax = 3;
            variableFunctionLimitations.Ymin = -2;
            variableFunctionLimitations.Ymax = 6;
            variableFunctionLimitations.ε = 0.001;
            variableFunctionLimitations.step = 0.1;
            variableFunctionLimitations.countComplex = 500;
            functionValue = new();
            Getchart3Ddata = new();
            Getchart2Ddata = new();
            _taskService = tasksService;
            _methodService = methodService;
            Getrnd = 6;
            Gettasks = new (_taskService.GetAllTask());
            GetcurrentTask = Gettasks[1];
            Getmethods = new (_methodService
                .GetAllOptimizationMethods()
                .Where(x=>x.IsRealized == true)
                .Select(el=>el));
            GetcurrentMethod = Getmethods[0];
            GetDescriptionPicture = string.Join("", GetcurrentTask.Name.Split(" ")) + ".png";
            GettaskParameters = new(_taskService
                .GetAllParametersValues()
                .Where(x => x.TaskId == currentTask.IdTask)
                .Select(el => el)
                .ToList());
        }
        #endregion

        #region команды
        private RelayCommand start;
        private RelayCommand build3DChart;
        private RelayCommand clear;
        private RelayCommand taskDescription;
        private RelayCommand reference;
        private RelayCommand saveresult;
        public ICommand Start
        {
            get
            {
                return start ??
                  (start = new RelayCommand(obj =>
                  {
                      if (Getxmax <= Getxmin || Getymax <= Getymin) 
                      {
                          MessageBox.Show("Неверно заданы ограничения области", "Ошибка");
                          return;
                      }
                      foreach(ITask task in Tasklist.tasks)
                      {
                          foreach (var name in task.Name)
                          {
                              if (name == currentTask.Name)
                              {
                                  foreach (IMethod method in Methodlist.methods)
                                  {
                                      if (method.Name == currentMethod.Name)
                                      {
                                          GetfunctionValue.Clear();
                                          task.RegisterTask(new List<TaskParameterValueView>(taskParameters));
                                          if (Getextremum == "условный максимум")
                                          {
                                              variableFunctionLimitations.TypeOfExtremum = true;
                                              method.RegisterMethod(variableFunctionLimitations, task.GetTask);
                                          }
                                          if (Getextremum == "условный минимум")
                                          {
                                              variableFunctionLimitations.TypeOfExtremum = false;
                                              method.RegisterMethod(variableFunctionLimitations, task.GetTask);
                                          }
                                          try { GetfunctionValue.Add(method.Solve()); }
                                          catch 
                                          {
                                              MessageBox.Show("Неверно заданы некоторые параметры функции", "Ошибка");
                                              return;
                                          }
                                          var res = GetfunctionValue[0].Round(this.rnd);
                                          Getchart3Ddata = method.GetChartData();
                                          Getchart2Ddata = method.GetChartLimitationData();
                                          Getresult = $"Значение целевой функции в точке\n" +
                                          $"X = {res.X}\n" +
                                          $"Y = {res.Y}\n" +
                                          $"Z(X, Y) = {res.FunctionValue}";
                                          exceldata = method.GetChartDataAsTable();
                                          return;
                                      }
                                  }
                              }
                          }
                      }
                      MessageBox.Show("Задача или метод оптимизации не найдены", "Ошибка");
                  }));
            }
        }
        public ICommand Clear
        {
            get
            {
                return clear ??
                  (clear = new RelayCommand(obj =>
                  {
                      GetfunctionValue.Clear();
                      Getchart3Ddata.Clear();
                      Getchart2Ddata.Clear();
                      Getresult = "";
                  }));
            }
        }
        public ICommand Build3DChart
        {
            get
            {
                return build3DChart ??
                  (build3DChart = new RelayCommand(obj =>
                  {
                      var builderBase = Container.GetBuilder().Build();
                      Chart3D Chart3D = new() {
                          DataContext = builderBase.Resolve<Chart3DViewModel>(new NamedParameter("p1", chart3Ddata))
                      };
                      Chart3D.ShowDialog();
                  }));
            }
        }
        public ICommand TaskDescription
        {
            get
            {
                return taskDescription ??
                  (taskDescription = new RelayCommand(obj =>
                  {
                      var builderBase = Container.GetBuilder().Build();
                      TaskDescription TaskDescription = new()
                      {
                          DataContext = builderBase.Resolve<TaskDescriptionViewModel>(new NamedParameter("p1", currentTask.Description))
                      };
                      TaskDescription.Show();
                  }));
            }
        }
        public ICommand Reference
        {
            get
            {
                return reference ??
                  (reference = new RelayCommand(obj =>
                  {
                      string message = "Программный комплекс для решения задач оптимизации";
                      MessageBox.Show(message, "Справка");
                  }));
            }
        }
        public ICommand Saveresult
        {
            get
            {
                return saveresult ??
                  (saveresult = new RelayCommand(obj =>
                  {
                      IApplication application = (new ExcelEngine()).Excel;
                      if (application == null)
                      {
                          MessageBox.Show("Excel не установлен на вашем устройстве", "Ошибка");
                          return;
                      }
                      if (exceldata == null)
                      {
                          MessageBox.Show("Недостаточно данных", "Ошибка");
                          return;
                      }
                      var builderBase = Container.GetBuilder().Build();
                      dialogService = builderBase.Resolve<DialogService>();
                      fileService = builderBase.Resolve<FileService>();
                      string data =
                      $"Задача: {currentTask.Name}\n" +
                      $"Метод: {currentMethod.Name}\n" +
                      $"Значение целевой функции в точке\n" +
                      $"X = {GetfunctionValue[0].X}\n" +
                      $"Y = {GetfunctionValue[0].Y}\n" +
                      $"F(X, Y) = {GetfunctionValue[0].FunctionValue}";
                      SaveFile.SaveXls(exceldata, data, application);
                      try
                      {
                          if (!dialogService.SaveFileDialog()) { return; }
                          fileService.Save(dialogService.FilePath, application);
                      }
                      catch
                      {
                          string message = "Не удалось сохранить файл.";
                          MessageBox.Show(message, "Ошибка");
                      }

                  }));
            }
        }
        #endregion
    }
}
