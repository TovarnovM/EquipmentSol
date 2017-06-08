using EquipWPF;
using GameLoop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ForLuda {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        MainViewModel_1 vm;
        public MainWindow() {
            InitializeComponent();
            vm = DataContext as MainViewModel_1;
            LoadFromString(Properties.Resources._def);
            
            // LoadFromCSV(@"C:\Users\User\Desktop\default.csv");
        }

        List<RectangleP> lst = new List<RectangleP>();
        private double p_por;
        private int nVsego;
        private int nShow;

        delegate void SetterDeleg(double value);
        void LoadFromCSV(string FilePath) {
            LoadFromString(File.ReadAllText(FilePath));
        }
        void LoadFromString(string BigString) {
            try {
                var lines = BigString.Split('\n');
                var l = new List<RectangleP>(lines.Length);
                var zag = lines[0].Split(';').Select(z => z.Trim().ToUpper()).ToArray();
                
                foreach (var item in lines.Skip(1)) {
                    var r = new RectangleP();
                    var ss = item.Split(';');
                    if (ss.Length != zag.Length)
                        continue;
                    var dict = r
                        .GetType()
                        .GetProperties()
                        .ToDictionary(
                            p => p.Name,
                            p => Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(p.PropertyType), r, p.GetSetMethod(true))
                         );
                    for (int i = 0; i < zag.Length; i++) {
                        dict[zag[i]].DynamicInvoke(GetDouble(ss[i]));
                    }
                    l.Add(r);

                }

                lst.Clear();
                lst = l;
                DG.ItemsSource = null;
                DG.ItemsSource = lst;
                ShowTarget();
            } catch (Exception e) {
                MessageBox.Show("Ошибка при чтении файла");

                throw;
            }

        }
        public static double GetDouble(string value, double defaultValue = 0d) {

            //Try parsing in the current culture
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out double result) &&
                //Then try in US english
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                //Then in neutral language
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {
                result = defaultValue;
            }

            return result;
        }

        private void b2_Click(object sender, RoutedEventArgs e) {
            var od = new OpenFileDialog();
            if(od.ShowDialog() == true) {
                LoadFromCSV(od.FileName);
            }
            
        }
        Tank GetFighter() {
            var f = new Tank();
            f.AimSurf.AimPoint = new Vector(GetDouble(HP1.Text), GetDouble(reload1_Min.Text));
            foreach (var rp in lst) {
                var xmin = Math.Min(rp.X1, rp.X2);
                var xmax = Math.Max(rp.X1, rp.X2);
                var ymin = Math.Min(rp.Y1, rp.Y2);
                var ymax = Math.Max(rp.Y1, rp.Y2);
                f.AimSurf.Boxes.Add(new GameLoop.Rect(xmin, ymin, xmax, ymax, rp.P));
            }
            return f;
        }
        void ShowTarget(Tank f = null) {
            f = f != null ? f :GetFighter();
            vm.DrawAim(vm.Model1, f,nShow,nVsego,p_por);
            
        }

        private void b2_Copy_Click(object sender, RoutedEventArgs e) {
            ShowTarget();
        }

        private async void button_Click(object sender, RoutedEventArgs e) {
            var oldC = Cursor;
            Cursor = Cursors.Wait;
            int n = (int)GetDouble(low1.Text);
            double SKO_x = GetDouble(Ex1.Text);
            double SKO_y = GetDouble(Ey1.Text);
            var f = GetFighter();
            GoBtn.IsEnabled = false;
            await StartSimAsync(f, n, SKO_x, SKO_y);
            nShow = (int)GetDouble(betweenQeues1_Min.Text);
            ShowTarget(f);
            GoBtn.IsEnabled = true;
            Cursor = oldC;
        }

        private Task StartSimAsync(Tank f, int n, double sKO_x, double sKO_y) {
            return Task.Factory.StartNew(() => StartSim(f, n, sKO_x, sKO_y));
        }

        private void StartSim(Tank f, int n, double sKO_x, double sKO_y) {
            for (int i = 0; i < n; i++) {
                double x = Rnd.GetNorm(f.AimSurf.AimPoint.X, sKO_x, false);
                double y = Rnd.GetNorm(f.AimSurf.AimPoint.Y, sKO_y, false);
                var vec = new Vector(x, y);
                f.Hits.Add(new Tuple<double, Vector>(f.AimSurf.getDamage(vec), vec));
            }

            nVsego = n;
            p_por = f.Hits.Select(tp => tp.Item1).Average();
        }
        public TmpRnd Rnd { get; set; } = new TmpRnd();
    }

    public class RectangleP {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public double P { get; set; }
        public RectangleP() {

        }
        public RectangleP(double x1, double y1, double x2, double y2, double p) {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
            P = p;
        }
    }

    public class Tank {
        public List<Tuple<double, Vector>> Hits = new List<Tuple<double, Vector>>();
        public IAimSurface AimSurf { get; set; } = new AimSurf();
        public string Name { get; set; } = "Танк";

    }
}
