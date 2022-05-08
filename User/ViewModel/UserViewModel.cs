using Autofac;
using AutofacDependence;
using ModelsView;
using ReactiveUI;
using Repository.factories;
using Services;
using Services.Interfaces;
using ServicesMVVM;
using System.Collections.ObjectModel;
using System.Linq;
using User.View;
using System.Windows.Input;
using User.Model;
using Xceed.Wpf.Toolkit;

namespace User.ViewModel
{
    internal class UserViewModel : ReactiveObject
    {
        #region поля
        private ObservableCollection<string> listSing;
        private ObservableCollection<Point2> functionValue;
        private ObservableCollection<Point3> chart3Ddata;
        private ObservableCollection<Point2> chart2Ddata;
        private ObservableCollection<TaskView> tasks;
        private ObservableCollection<OptimizationMethodView> methods;
        private ObservableCollection<TaskParameterValueView> taskParameters;
        private OptimizationMethodView currentMethod;
        private TaskView currentTask;
        private double k;
        private double b;
        private double xmin;
        private double xmax;
        private double ymin;
        private double ymax;
        private string sing;
        private string result;
        private double ε;

        #endregion

        #region get; set
        public double Getk
        {
            get { return k; }
            set { this.RaiseAndSetIfChanged(ref k, value); }
        }
        public double Getb
        {
            get { return b; }
            set { this.RaiseAndSetIfChanged(ref b, value); }
        }
        public double Getxmin
        {
            get { return xmin; }
            set { this.RaiseAndSetIfChanged(ref xmin, value); }
        }
        public double Getxmax
        {
            get { return xmax; }
            set { this.RaiseAndSetIfChanged(ref xmax, value); }
        }
        public double Getymin
        {
            get { return ymin; }
            set { this.RaiseAndSetIfChanged(ref ymin, value); }
        }
        public double Getymax
        {
            get { return ymax; }
            set { this.RaiseAndSetIfChanged(ref ymax, value); }
        }
        public ObservableCollection<string> GetlistSing
        {
            get { return listSing; }
            set { this.RaiseAndSetIfChanged(ref listSing, value); }
        }
        public ObservableCollection<Point3> Getchart3Ddata
        {
            get { return chart3Ddata; }
            set { this.RaiseAndSetIfChanged(ref chart3Ddata, value); }
        }
        public ObservableCollection<Point2> Getchart2Ddata
        {
            get { return chart2Ddata; }
            set { this.RaiseAndSetIfChanged(ref chart2Ddata, value); }
        }
        public ObservableCollection<Point2> GetfunctionValue
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
            set { this.RaiseAndSetIfChanged(ref currentTask, value); }
        }
        public string Getsing
        {
            get { return sing; }
            set { this.RaiseAndSetIfChanged(ref sing, value); }
        }
        public string Getresult
        {
            get { return result; }
            set { this.RaiseAndSetIfChanged(ref result, value); }
        }
        public double Getε
        {
            get { return ε; }
            set { this.RaiseAndSetIfChanged(ref ε, value); }
        }
        #endregion

        #region конструкторы
        public UserViewModel()
        {
            GetlistSing = new() { ">", "<", "⩾", "⩽" };
            Getsing = "⩾";
            Getk = 1;
            Getb = 2;
            Getxmin = -3;
            Getxmax = 0;
            Getymin = -0.5;
            Getymax = 3;
            Getε = 0.01;
            GetfunctionValue = new();
            ContainerBuilder builderBase = new();
            builderBase.RegisterModule(new RepositoryModule());
            var sql = builderBase.Build().Resolve<ISqlLiteRepositoryContextFactory>();
            ITasksService taskservice = new TasksService(sql);
            IMethodService methodservice = new MethodService(sql);
            Gettasks = new (taskservice.GetAllTask());
            GetcurrentTask = Gettasks[0];
            Getmethods = new (methodservice.GetAllOptimizationMethods().Where(x=>x.IsRealized == true).Select(el=>el));
            GetcurrentMethod = Getmethods[0];
            taskParameters = new(taskservice.GetAllParametersValues());
        }
        #endregion

        #region команды
        private RelayCommand start;
        private RelayCommand build3DChart;
        private RelayCommand clear;
        private RelayCommand taskDescription;
        private RelayCommand reference;
        public ICommand Start
        {
            get
            {
                return start ??
                  (start = new RelayCommand(obj =>
                  {
                      if (xmax <= xmin || ymax <= ymin) 
                      {
                          MessageBox.Show("Неверно заданы ограничения области", "Ошибка");
                          return;
                      }
                      foreach(ITask task in Tasklist.tasks)
                      {
                          if(task.Name == currentTask.Name)
                          {
                              foreach(IMethod method in Methodlist.methods)
                              {
                                  if (method.Name == currentMethod.Name) 
                                  {
                                      GetfunctionValue.Clear();
                                      var parameters = taskParameters.Where(x => x.TaskId == currentTask.IdTask).Select(el => el).ToList();
                                      task.RegisterTask(parameters);
                                      method.RegisterMethod(true, k, b, sing, xmin, xmax, ymin, ymax, ε, task.GetTask);
                                      GetfunctionValue.Add(method.Solve());
                                      Getchart3Ddata = method.GetChartData();
                                      Getchart2Ddata = method.GetChartLimitationData();
                                      Getresult = $"Значение целевой функции в точке\n" +
                                      $"X = {GetfunctionValue[0].X}\n" +
                                      $"Y = {GetfunctionValue[0].Y}\n" +
                                      $"F(X, Y) = {GetfunctionValue[0].FunctionValue}";
                                      return;
                                  }
                              }
                              break;
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
        #endregion
    }
}
