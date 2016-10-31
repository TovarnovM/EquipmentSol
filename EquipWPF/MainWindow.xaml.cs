using System;
using System.Collections.Generic;
using System.Linq;
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

namespace EquipWPF {
    public struct sdata {
        public double lowborder;//нижяя граница времени до 1 попадания
        public double topborder;//верхняя
        public double betweenQeue_Min;//время вежду очередями
        public double betweenQeue_Max;//время вежду очередями
        public double HP;//жызни
        public double reload_Min;//время перезарядки
        public double reload_Max;//время перезарядки
        public double Ex;
        public double Ey;
        public double Bx;
        public double By;
        public double mindist;//мин расстояние
        public double maxdist;//макс расстояние
        public double stepcount;//количество рассчетов на каждом шаге расстояния
        public double deltadist;//шаг расстояния
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void button_Click(object sender,RoutedEventArgs e) {
            Loaddata();
            var vm = DataContext as MainViewModel;
            vm.OneSampleTest();

            //vm.Progress += 5;
            //vm.Maximum += 5;
            
            //button.Content = $"{ vm.Progress}  {pb.Value}  {vm.Maximum} {pb.Maximum}";
        }

        private void button2_Click(object sender,RoutedEventArgs e) {

        }

        private void button3_Click(object sender,RoutedEventArgs e) {
            Loaddata();
            var vm = DataContext as MainViewModel;
            vm.Start();
        }

        private void button3_Copy_Click(object sender,RoutedEventArgs e) {

            var vm = DataContext as MainViewModel;
            vm.UpdateStats();
        }

        public void Loaddata() {
            sdata we, enemy;
            Double.TryParse(low1.Text,out we.lowborder);

            Double.TryParse(top1.Text,out we.topborder);

            Double.TryParse(betweenQeues1_Min.Text,out we.betweenQeue_Min);
            Double.TryParse(betweenQeues1_Max.Text,out we.betweenQeue_Max);

            Double.TryParse(HP1.Text,out we.HP);

            Double.TryParse(reload1_Min.Text,out we.reload_Min);
            Double.TryParse(reload1_Max.Text,out we.reload_Max);

            Double.TryParse(Ex1.Text,out we.Ex);

            Double.TryParse(Ey1.Text,out we.Ey);

            Double.TryParse(Bx.Text,out we.Bx);
            Double.TryParse(By.Text,out we.By);

            Double.TryParse(mindist1.Text,out we.mindist);

            Double.TryParse(maxdist1.Text,out we.maxdist);

            Double.TryParse(stepcount1.Text,out we.stepcount);

            Double.TryParse(deltadist1.Text,out we.deltadist);



            Double.TryParse(low2.Text,out enemy.lowborder);

            Double.TryParse(top2.Text,out enemy.topborder);

            Double.TryParse(betweenQeues2_Min.Text,out enemy.betweenQeue_Min);
            Double.TryParse(betweenQeues2_Max.Text,out enemy.betweenQeue_Max);

            Double.TryParse(HP2.Text,out enemy.HP);

            Double.TryParse(reload2_Min.Text,out enemy.reload_Min);
            Double.TryParse(reload2_Max.Text,out enemy.reload_Max);

            Double.TryParse(Ex2.Text,out enemy.Ex);

            Double.TryParse(Ey2.Text,out enemy.Ey);

            Double.TryParse(Bx2.Text,out enemy.Bx);
            Double.TryParse(By2.Text,out enemy.By);

            enemy.mindist = we.mindist;
            enemy.maxdist = we.maxdist;
            enemy.stepcount = we.stepcount;
            enemy.deltadist = we.stepcount;


            var vm = DataContext as MainViewModel;
            vm.LoadData(we, enemy);
        }

        private void Window_Closed(object sender,EventArgs e) {
            var vm = DataContext as MainViewModel;
            vm.CancelTasks();
        }

        /*
         using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        sdata first,second;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Double.TryParse(low1.Text, out first.lowborder);

            Double.TryParse(top1.Text, out first.topborder);

            Double.TryParse(betweenQeues1.Text, out first.betweenQeue);

            Double.TryParse(HP1.Text, out first.HP);

            Double.TryParse(reload1.Text, out first.reload);
            
            Double.TryParse(Ex1.Text, out first.Ex);
            
            Double.TryParse(Ey1.Text, out first.Ey);
            
            Double.TryParse(deviation1.Text, out first.deviation);
            
            Double.TryParse(mindist1.Text, out first.mindist);
            
            Double.TryParse(maxdist1.Text, out first.maxdist);
            
            Double.TryParse(stepcount1.Text, out first.stepcount);
            
            Double.TryParse(deltadist1.Text, out first.deltadist);
        }


        private void button2_Click ( object sender, RoutedEventArgs e ) {
            Double.TryParse(low2.Text, out second.lowborder);

            Double.TryParse(top2.Text, out second.topborder);

            Double.TryParse(betweenQeues2.Text, out second.betweenQeue);

            Double.TryParse(HP2.Text, out second.HP);

            Double.TryParse(reload2.Text, out second.reload);

            Double.TryParse(Ex2.Text, out second.Ex);

            Double.TryParse(Ey2.Text, out second.Ey);

            Double.TryParse(deviation2.Text, out second.deviation);

            Double.TryParse(mindist2.Text, out second.mindist);

            Double.TryParse(maxdist2.Text, out second.maxdist);

            Double.TryParse(stepcount2.Text, out second.stepcount);

            Double.TryParse(deltadist2.Text, out second.deltadist);
        }
    }
}

         */
    }
}
