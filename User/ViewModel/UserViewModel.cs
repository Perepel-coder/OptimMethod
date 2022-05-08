using Autofac;
using ReactiveUI;
using ServicesMVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;
using User.Model;

namespace User.ViewModel
{
    public class UserViewModel : ReactiveObject
    {
        #region поля
        private IContainer container = Container.GetBuilder().Build();
        private ComplexBoxingMethod complexBoxingMethod;
        private ObservableCollection<string> listSing;
        private ObservableCollection<Point3> chart3Ddata;
        private ObservableCollection<Point2> chart2Ddata;
        private double k;
        private double b;
        private double xmin;
        private double xmax;
        private double ymin;
        private double ymax;
        private string sing;
        private string result;
        private double ε;
        private ObservableCollection<Point2> functionValue;
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
        }
        #endregion

        #region команды
        private RelayCommand start;
        private RelayCommand build3DChart;
        private RelayCommand clear;
        public ICommand Start
        {
            get
            {
                return start ??
                  (start = new RelayCommand(obj =>
                  {
                      GetfunctionValue = new();
                      complexBoxingMethod = new(true, k, b, sing, xmin, xmax, ymin, ymax, ε);
                      GetfunctionValue.Add(complexBoxingMethod.Solve());
                      Getchart3Ddata = complexBoxingMethod.GetChartData(Task.GetTask15);
                      Getchart2Ddata = complexBoxingMethod.GetChartLimitationData();
                      Getresult = $"Значение целевой функции в точке\n" +
                      $"X = {GetfunctionValue[0].X}\n" +
                      $"Y = {GetfunctionValue[0].Y}\n" +
                      $"F(X, Y) = {GetfunctionValue[0].FunctionValue}";
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
                      var Chart3D = new Chart3D { 
                          DataContext = container.Resolve<Chart3DViewModel>(new NamedParameter("p1", chart3Ddata)) 
                      };
                      Chart3D.ShowDialog();
                  }));
            }
        }
        #endregion
    }
}
