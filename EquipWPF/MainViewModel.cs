using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipWPF {
    public class MainViewModel: INotifyPropertyChanged {
        public MainViewModel() {
            Model1 = GetNewModel("Enemy");
            Model1.PlotType = PlotType.Cartesian;
            Model2 = GetNewModel("Me");
            Model2.PlotType = PlotType.Cartesian;
        }
        public PlotModel Model1 { get; set; }
        public PlotModel Model2 { get; set; }
        
        public PlotModel GetNewModel(string title = "") {
            var m = new PlotModel { Title = title };
            var linearAxis1 = new LinearAxis();
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MaximumPadding = 0;
            linearAxis1.MinimumPadding = 0;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            linearAxis1.Position = AxisPosition.Bottom;
            m.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MaximumPadding = 0;
            linearAxis2.MinimumPadding = 0;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            m.Axes.Add(linearAxis2);
            return m;
        }

        #region ProgressBar
        private double _min = 0;
        public double Minimum {
            get { return _min; }
            set {
                _min = value;
                OnPropertyChanged("Minimum");
            }
        }
        private double _max = 100;
        public double Maximum {
            get { return _max; }
            set {
                _max = value;
                OnPropertyChanged("Maximum");
            }
        }
        private double _progress;
        public double Progress {
            get { return _progress; }
            set {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
