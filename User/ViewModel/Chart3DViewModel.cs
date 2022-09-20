using OptimizationMethods;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace User.ViewModel
{
    internal class Chart3DViewModel : ReactiveObject
    {
        private ObservableCollection<Point3> chart3Ddata;
        public ObservableCollection<Point3> Getchart3Ddata
        {
            get { return chart3Ddata; }
            set { this.RaiseAndSetIfChanged(ref chart3Ddata, value); }
        }
        public Chart3DViewModel(ObservableCollection<Point3> data)
        {
            Getchart3Ddata = data;
        }
    }
}
