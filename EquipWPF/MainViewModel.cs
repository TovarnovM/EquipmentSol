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
using System.Threading;

namespace EquipWPF {
    public class MainViewModel: INotifyPropertyChanged {
        public MainViewModel() {
            Model1 = GetNewModel("Enemy");
            Model1.PlotType = PlotType.Cartesian;
            Model2 = GetNewModel("Me");
            Model2.PlotType = PlotType.Cartesian;

            Model3 = GetNewModel("Stats");
            Model3.Series.Add(new LineSeries() {
                Color = OxyColors.Green,
                Title = "We won",
                StrokeThickness = 3
            });
            Model3.Series.Add(new LineSeries() {
                Color = OxyColors.Red,
                Title = "Enemy won",
                StrokeThickness = 3
            });


            Standing = new AimSurf();
            Standing.AddBox(-0.15 * 0.5,1.9 - 0.280,0.15 * 0.5,1.9,0.9);

            Standing.AddBox(-0.35 * 0.5,0.55 + 0.42,0.35 * 0.5,0.55 + 0.42 + 0.65,0.5);

            Standing.AddBox(-0.35 * 0.5,0.55,-0.35 * 0.5 + 0.15,0.55 + 0.42,0.4);
            Standing.AddBox(0.35 * 0.5 - 0.15,0.55,0.35 * 0.5,0.55 + 0.42,0.4);

            Standing.AddBox(-0.35 * 0.5 + 0.02,0,-0.35 * 0.5 + 0.11 + 0.02,0.55,0.3);
            Standing.AddBox(0.35 * 0.5 - 0.11 - 0.02,0,0.35 * 0.5  - 0.02,0.55,0.3);

            Standing.AddBox(-0.35 * 0.5 - 0.11,0.55 + 0.42 + 0.65 - 0.75,-0.35 * 0.5,0.55 + 0.42 + 0.65,0.3);
            Standing.AddBox(0.35 * 0.5,0.55 + 0.42 + 0.65 - 0.75,0.35 * 0.5 + 0.11,0.55 + 0.42 + 0.65,0.3);
            Standing.AimPoint = new Vector(0,0.55 + 0.42 + 0.65 * 0.7);
        }
        public PlotModel Model1 { get; set; }
        public PlotModel Model2 { get; set; }
        public PlotModel Model3{ get; set; }
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


        public double TimeToShootWe { get; set; } = 1d;
        public double TimeToShootEnemy { get; set; } = 1.2d;
        public double HPwe { get; set; } = 1;
        public double HPenemy { get; set; } = 1;
        public double Ex_we { get; set; } = 1d / 1000d;
        public double Ey_we { get; set; } = 1d / 10000d;
        public double Ex_enemy { get; set; } = 1d / 1000d;
        public double Ey_enemy { get; set; } = 1d / 10000d;

        public AimSurf Standing { get; set; }

        private Fighter GetWe(double distance = 0d) 
        {
                var we = new Fighter("We");
            we.AimSurf = Standing.CopyMe();
            we.Weapon = WeaponFactory.Get("AK74");
            we.TimeToNextLine = TimeToShootWe;
            we.Pos = new Vector(distance,0);
            we.HP = HPwe;
            we.Ex = Ex_we;
            we.Ey = Ey_we;
            return we;

        }
        private Fighter GetEnemy(double distance = 200d) {
            var enemy = new Fighter("enemy");
            enemy.AimSurf  = Standing.CopyMe();
            enemy.Weapon = WeaponFactory.Get("AK74");
            enemy.TimeToNextLine = TimeToShootEnemy;
            enemy.Pos = new Vector(distance,0);
            enemy.HP = HPenemy;
            enemy.Ex = Ex_enemy;
            enemy.Ey = Ey_enemy;
            return enemy;

        }

        public GLEnviroment GetOneTest(double distance) {
            var we = GetWe();

            var enemy = GetEnemy(distance);

            enemy.Target = we;
            we.Target = enemy;


            var env = new GLEnviroment();
            env.AddUnit(we);
            env.AddUnit(enemy);
            env.StopFunc += () => {
                return we.Dead || enemy.Dead;
            };
            env.dT = 0.001;
            return env;
        }

        public void OneSampleTest() {

            var we = GetWe();

            var enemy = GetEnemy(200);

            enemy.Target = we;
            we.Target = enemy;

    
            var env = new GLEnviroment();
            env.AddUnit(we);
            env.AddUnit(enemy);
            env.StopFunc += () => {
                return we.Dead || enemy.Dead;
            };
            env.dT = 0.001;

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

        #region TAASKS
        public List<Task<GLEnviroment>> Tasks { get; set; } = new List<Task<GLEnviroment>>(1000);
        public CancellationTokenSource CTS { get; set; } = new CancellationTokenSource();

        public double DistanceMin { get; set; } = 10d;
        public double DistanceMax { get; set; } = 500d;
        public double DistanceShag { get; set; } = 10d;
        public int NIter { get; set; } = 10000;

        private object _lock = new object();

        public void Start(bool hits = true) {
            if(Tasks.Count > 0) {
                CTS.Cancel();
                lock(_lock) {
                    Tasks.Clear();
                }
                
            }

            Progress = 0;
            Minimum = 0;
            Maximum = Math.Ceiling((DistanceMax - DistanceMin) / DistanceShag) * NIter;
            Tasks.Capacity = (int)Maximum + 1;
            CTS = new CancellationTokenSource();
            var ct = CTS.Token;
            var dist = DistanceMin;

            Task.Factory.StartNew(() => {
                bool lastLap = false;
                var lmbda = new Func<object,GLEnviroment>((d) => {
                    var env = GetOneTest((double)d);
                    env.dT = 0.01;
                    env.MaxTime = 60;
                    env.Start();
                    if(hits) {
                        (env.Units[0] as Fighter).ClearStats();
                        (env.Units[1] as Fighter).ClearStats();
                    }

                    return env;
                });

                while(dist <= DistanceMax || !lastLap) {
                    lock(_lock) {
                        for(int i = 0; i < NIter; i++) {
                            var tsk = Task.Factory.StartNew<GLEnviroment>(lmbda, dist, ct);
                            tsk.ContinueWith((a) => {
                                Progress++;
                            });
                            Tasks.Add(tsk);
                        }
                    }

                    //Tasks.Last().ContinueWith((env) => UpdateStats());

                    dist += DistanceShag;
                    if(dist == DistanceMax)
                        continue;
                    if(!lastLap && dist > DistanceMax) {
                        lastLap = true;
                        dist = DistanceMax;
                    }
                
                }

            }, ct);


            var ss = 1;   
        }
        public void UpdateStats() {
            lock(_lock) {


                var serWe = Model3.Series[0] as LineSeries;
                var serEnemy = Model3.Series[1] as LineSeries;

                serEnemy.Points.Clear();
                serWe.Points.Clear();

                var readydata = from tsk in Tasks
                                where tsk.IsCompleted
                                group tsk by (double)tsk.AsyncState into dists1
                                select new {
                                    Dist = dists1.Key,
                                    Battles = dists1.ToList()
                                };
                var dists = readydata.ToList();
            
            foreach(var dist in dists) {
                var perWeWon = (double)dist.Battles.Where((t) => (t.Result.Units.First((u) => u.Name == "We") as Fighter).HP>0 ).Count() / dist.Battles.Count();
                serWe.Points.Add(new DataPoint(dist.Dist,perWeWon));
                serEnemy.Points.Add(new DataPoint(dist.Dist,1-perWeWon));
            }}
            Model3.InvalidatePlot(true);
            


        }


        #endregion

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
