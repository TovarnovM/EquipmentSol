using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLoop;
using OxyPlot.Series;
using System.Windows;
using OxyPlot.Annotations;

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

        public void OneTest() {
            
            var we = new Fighter("We");
            var surf = new AimSurf();
            surf.AddBox(-0.1,0,0.1,1,0.1);
            surf.AimPoint = new Vector(0,0.5);
            we.AimSurf = surf;
            we.Weapon = WeaponFactory.Get("AK74");

            var enemy = new Fighter("enemy");
            surf = new AimSurf();
            surf.AddBox(-0.2,0,0.2,1,1);
            surf.AddBox(-0.1,0.2,0.1,0.5,0.1);
            surf.AimPoint = new Vector(0,0.5);
            enemy.AimSurf = surf;
            enemy.Weapon = WeaponFactory.Get("AK74");

            enemy.Target = we;
            we.Target = enemy;

            enemy.TimeToNextLine = 3;
            we.TimeToNextLine = 1;

            we.Pos = new Vector(0,0);
            enemy.Pos = new Vector(200,0);

            we.HP = 100;
            enemy.HP = 200;


            var env = new GLEnviroment();
            env.AddUnit(we);
            env.AddUnit(enemy);
            env.StopFunc += () => {
                return we.Dead || enemy.Dead;
            };
            env.dT = 0.0001;

            env.Start();


            DrawAim(Model1,enemy);
            DrawAim(Model2,we);


        }

        public void DrawAim(PlotModel pm, Fighter f) {
            pm.Title = $"Name = {f.Name} ;  HP = {f.HP} ;  Dead = {f.Dead}";


            pm.Series.Clear();
            var ss = new ScatterSeries() {
                MarkerType = MarkerType.Star,
                MarkerSize = 4,
                MarkerFill = OxyColors.Red,
                MarkerStroke = OxyColors.Red,
                MarkerStrokeThickness = 1,
               
               
            };
            foreach(var point in f.Hits) {
                var sp = new ScatterPoint(point.Item2.X,point.Item2.Y,value: point.Item1,tag: "t = " + point.Item1.ToString("G1"));
        
                ss.Points.Add(sp);
            }
            pm.Series.Add(ss);

            pm.Annotations.Clear();

            foreach(var box in f.AimSurf.Boxes) {
                var ra = new RectangleAnnotation() {
                    MinimumX = box.xmin,
                    MaximumX = box.xmax,
                    MinimumY = box.ymin,
                    MaximumY = box.ymax,
                    Fill = OxyColors.Blue.ChangeSaturation(box.damage),
                    Layer = AnnotationLayer.BelowSeries
                };
                pm.Annotations.Add(ra);
            }

            var ap = new PointAnnotation() {
                X = f.AimSurf.AimPoint.X,
                Y = f.AimSurf.AimPoint.Y,
                Shape = MarkerType.Plus,
                Stroke = OxyColors.Green,
                StrokeThickness = 3,
                Size = 7,
                Text = $"({f.AimSurf.AimPoint})",
                Layer = AnnotationLayer.AboveSeries
            };

            pm.Annotations.Add(ap);


            pm.InvalidatePlot(true);



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
