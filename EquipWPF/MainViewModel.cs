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
                Title = "We won, %",
                StrokeThickness = 3
            });
            Model3.Series.Add(new LineSeries() {
                Color = OxyColors.Red,
                Title = "Enemy won, %",
                StrokeThickness = 3
            });
            Model3.Series.Add(new LineSeries() {
                Color = OxyColors.Bisque,
                Title = "TimeAver, s",
                StrokeThickness = 3
            });
            Model3.Series.Add(new LineSeries() {
                Color = OxyColors.DarkSeaGreen,
                Title = "OmegaAver we",
                StrokeThickness = 3
            });
            Model3.Series.Add(new LineSeries() {
                Color = OxyColors.MediumVioletRed,
                Title = "OmegaAver enemy",
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


        public double TimeToShootWe0 { get; set; } = 1d;
        public double TimeToShootEnemy0 { get; set; } = 1.2d;
        public double TimeToShootWe1 { get; set; } = 1d;
        public double TimeToShootEnemy1 { get; set; } = 1.2d;
        public double HPwe { get; set; } = 1;
        public double HPenemy { get; set; } = 1;
        public double Bx_we { get; set; } = 1d / 1000d;
        public double By_we { get; set; } = 1d / 10000d;
        public double Bx_enemy { get; set; } = 1d / 1000d;
        public double By_enemy { get; set; } = 1d / 10000d;

        public double Ex_we { get; set; } = 1d / 1000d;
        public double Ey_we { get; set; } = 1d / 10000d;
        public double Ex_enemy { get; set; } = 1d / 1000d;
        public double Ey_enemy { get; set; } = 1d / 10000d;

        public AimSurf Standing { get; set; }

        private Fighter GetWe(double distance = 0d) 
        {
                var we = new Fighter("We");
            we.AimSurf = Standing.CopyMe();
            WeaponBase weap = WeaponFactory.Get("AK74") as WeaponBase;
            weap.Ex_line = Ex_we;
            weap.Ey_line = Ey_we;
            we.Weapon = weap;
            //we.TimeToNextLine = TimeToShootWe0;
            we.Pos = new Vector(distance,0);
            we.HP = HPwe;
            we.Bx = Bx_we;
            we.By = By_we;
            return we;

        }
        private Fighter GetEnemy(double distance = 200d) {
            var enemy = new Fighter("enemy");
            enemy.AimSurf  = Standing.CopyMe();
            WeaponBase weap = WeaponFactory.Get("AK74") as WeaponBase;
            weap.Ex_line = Ex_enemy;
            weap.Ey_line = Ey_enemy;
            enemy.Weapon = weap;
            //enemy.TimeToNextLine = TimeToShootEnemy0;
            enemy.Pos = new Vector(distance,0);
            enemy.HP = HPenemy;
            enemy.Bx = Bx_enemy;
            enemy.By = By_enemy;
            return enemy;

        }

        public GLEnviroment GetOneTest(double distance) {
     
            var we = GetWe();

            var enemy = GetEnemy(distance);


            we.TimeToNextLine = Rnd.GetDouble(TimeToShootWe0,TimeToShootWe1);
            enemy.TimeToNextLine = Rnd.GetDouble(TimeToShootEnemy0,TimeToShootEnemy1);

            enemy.Target = we;
            we.Target = enemy;


            var env = new GLEnviroment();
            env.AddUnit(we);
            env.AddUnit(enemy);
            env.StopFunc += () => {
                return !we.Dead && !enemy.Dead ? 0:
                we.Dead ? 1:
                enemy.Dead ? 2 : 3;
            };
            env.dT = 0.001;
            return env;
        }

        public void OneSampleTest() {

            var we = GetWe();

            var enemy = GetEnemy((DistanceMax + DistanceMin) *0.5);

            enemy.Target = we;
            we.Target = enemy;

    
            var env = new GLEnviroment();
            env.AddUnit(we);
            env.AddUnit(enemy);
            env.StopFunc += () => {
                return !we.Dead && !enemy.Dead ? 0 :
                we.Dead ? 1 :
                enemy.Dead ? 2 : 3;
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
        public class Resulter {
            public double Distance { get; set; }
            public double TimeAver { get; set; }
            public double TimeAver_we_won { get; set; }
            public double TimeAver_enemy_won { get; set; }
            public sdata We { get; set; }
            public sdata Enemy { get; set; }
            public double WeWonPerc { get; set; }
            public double EnemyWonPerc { get; set; }
            public double DrawPer { get; set; }
            //public double Ex_we_sum { get; set; }
            //public double Ey_we_sum { get; set; }
            //public double E_we_sum { get; set; }
            //public double Ey_enemy_sum { get; set; }
            //public double Ex_enemy_sum { get; set; }
            //public double E_enemy_sum { get; set; }
            public double Omega_we { get; set; }
            public double Omega_we_won { get; set; }
            public double Omega_we_loose { get; set; }
            public double Omega_enemy { get; set; }
            public double Omega_enemy_won { get; set; }
            public double Omega_enemy_loose { get; set; }

        }
        public List<Resulter> Tasks { get; set; } = new List<Resulter>(100);
        public CancellationTokenSource CTS { get; set; } = new CancellationTokenSource();

        public MyRandomGenerator.MyRandom Rnd { get; set; } = new MyRandomGenerator.MyRandom();
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
                    var tsk = Task.Factory.StartNew<Resulter>(
                        (st) => {
                            var res = new Resulter();
                            res.Distance = (double)st;
                            var lstenv = new List<GLEnviroment>(NIter + 1);
                            for(int i = 0; i < NIter; i++) {
                                var env = GetOneTest(res.Distance);
                                env.dT = 0.01;
                                env.MaxTime = 180;
                                env.Start();
                                Progress++;
                                lstenv.Add(env);
                            }
                            var wewon = (from e in lstenv
                                         where e.Result == 1
                                         select e).ToList();
                            var enemywon = (from e in lstenv
                                            where e.Result == 2
                                            select e).ToList();
                            var draw = lstenv.Except(wewon).Except(enemywon).ToList();
                            res.WeWonPerc = (double)wewon.Count / lstenv.Count;
                            res.EnemyWonPerc = (double)enemywon.Count / lstenv.Count;
                            res.DrawPer = (double)draw.Count / lstenv.Count;
                            res.Omega_we = lstenv.Select(e => e.Units[0] as Fighter).Aggregate(0d,(s,next) => s += next.Omega,(s) => s / lstenv.Count);
                            res.Omega_we_won = wewon.Select(e => e.Units[0] as Fighter).Aggregate(0d,(s,next) => s += next.Omega,(s) => s / wewon.Count);
                            res.Omega_we_loose = enemywon.Select(e => e.Units[0] as Fighter).Aggregate(0d,(s,next) => s += next.Omega,(s) => s / enemywon.Count);
                            res.Omega_enemy = lstenv.Select(e => e.Units[1] as Fighter).Aggregate(0d,(s,next) => s += next.Omega,(s) => s / lstenv.Count);
                            res.Omega_enemy_won = enemywon.Select(e => e.Units[1] as Fighter).Aggregate(0d,(s,next) => s += next.Omega,(s) => s / enemywon.Count);
                            res.Omega_enemy_loose = wewon.Select(e => e.Units[1] as Fighter).Aggregate(0d,(s,next) => s += next.Omega,(s) => s / wewon.Count);
                            res.TimeAver = lstenv.Aggregate(0d, (s,next)=>s+=next.Time, (s) => s/ lstenv.Count);
                            res.TimeAver_we_won = wewon.Aggregate(0d,(s,next) => s += next.Time,(s) => s / wewon.Count);
                            res.TimeAver_enemy_won = enemywon.Aggregate(0d,(s,next) => s += next.Time,(s) => s / enemywon.Count);
                            res.We = _we;
                            res.Enemy = _enemy;
                            return res;
                        },dist,ct);




                    

                        tsk.ContinueWith((a) => {
                            lock(_lock) {
                                Tasks.Add(a.Result);
                            }
                            UpdateStats();
                        });
                        
                        
                    

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

            var serWe = Model3.Series[0] as LineSeries;
            var serEnemy = Model3.Series[1] as LineSeries;
            var serT = Model3.Series[2] as LineSeries;
            var serOmegaWe = Model3.Series[3] as LineSeries;
            var serOmegaEnemy = Model3.Series[4] as LineSeries;
  

            serEnemy.Points.Clear();
            serWe.Points.Clear();
            serT.Points.Clear();
            serOmegaWe.Points.Clear();
            serOmegaEnemy.Points.Clear();

            List<Resulter> lstr;
            lock(_lock) {
                lstr = Tasks.OrderBy((r) => r.Distance).ToList();
            }
            foreach(var res in lstr) {
                serWe.Points.Add(new DataPoint(res.Distance,res.WeWonPerc * 100));
                serEnemy.Points.Add(new DataPoint(res.Distance,res.EnemyWonPerc * 100));
                serT.Points.Add(new DataPoint(res.Distance,res.TimeAver));
                serOmegaWe.Points.Add(new DataPoint(res.Distance,res.Omega_we));
                serOmegaEnemy.Points.Add(new DataPoint(res.Distance,res.Omega_enemy));
            }
            Model3.InvalidatePlot(true);
            


        }


        #endregion

        #region TAAASKS 2!!!!
        sdata _we, _enemy;
        public void LoadData(sdata we,sdata enemy) {
            _we = we;
            _enemy = enemy;
            TimeToShootWe0 = we.lowborder;
            TimeToShootEnemy0 = enemy.lowborder;
            TimeToShootWe1 = we.topborder;
            TimeToShootEnemy1 = enemy.topborder;
            HPwe = we.HP;
            HPenemy = enemy.HP;
            Bx_we = we.Bx/10000;
            By_we = we.By / 10000;
            Bx_enemy = enemy.Bx / 10000;
            By_enemy = enemy.By / 10000;

            Ex_we = we.Ex / 10000;
            Ey_we = we.Ey / 10000;
            Ex_enemy = enemy.Ex / 10000;
            Ey_enemy = enemy.Ey / 10000;

            DistanceMin = enemy.mindist;
            DistanceMax = enemy.maxdist;
            DistanceShag = enemy.deltadist;
            NIter = (int)enemy.stepcount;
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
