using GameLoop;
using Interpolator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipWPF {
    public class WeaponBase : IWeapon {
        public int RoundInMagazMax { get; set; }
        public int RoundInMagaz { get; set; }
        public MyRange<int> RoundInLine { get; set; }
        public MyRange<double> TimeBetweenLines { get; set; }
        public MyRange<double> ReloadTime { get; set; }
        public Func<double,double> GetDamage { get; set; }
        public double Ex_line { get; set; }
        public double Ey_line { get; set; }

        public string Name { get; set; }

        double IWeapon.Ex_line(double distance) {
            return Ex_line * distance / 100;
        }

        double IWeapon.Ey_line(double distance) {
            return Ex_line * distance / 100;
        }
    }

    public static class WeaponFactory {

        public static IWeapon Get(string weaponName) {
            switch(weaponName) {
                default:
                    return GetNewAK74();
            }
        }
        public static IWeapon GetNewAK74() {
            var ak = new WeaponBase() {
                Name = "AK74",
                RoundInMagazMax = 30,
                RoundInMagaz = 30,
                RoundInLine = new MyRange<int>(2,4),
                TimeBetweenLines = new MyRange<double>(2,3),
                ReloadTime = new MyRange<double>(4,6),
                Ex_line = 8E-2,
                Ey_line = 6E-2,
                GetDamage = (dist) => {
                    return dist < 1500 ? 1 : (dist < 3000 ? 0.5 : 0);
                }
            };
            //ak.Ex_line.Add(0,0);
            //ak.Ex_line.Add(100,8E-2);
            //ak.Ex_line.Add(200,16E-2);
            //ak.Ex_line.Add(300,24E-2);
            //ak.Ex_line.Add(400,32E-2);
            //ak.Ex_line.Add(500,40E-2);
            //ak.Ex_line.Add(600,48E-2);
            //ak.Ex_line.Add(700,56E-2);
            //ak.Ex_line.Add(800,64E-2);
            //ak.Ex_line.Add(900,73E-2);
            //ak.Ex_line.Add(1000,82E-2);

            //ak.Ey_line.Add(0,0);
            //ak.Ey_line.Add(100,6E-2);
            //ak.Ey_line.Add(200,12E-2);
            //ak.Ey_line.Add(300,18E-2);
            //ak.Ey_line.Add(400,24E-2);
            //ak.Ey_line.Add(500,30E-2);
            //ak.Ey_line.Add(600,36E-2);
            //ak.Ey_line.Add(700,42E-2);
            //ak.Ey_line.Add(800,48E-2);
            //ak.Ey_line.Add(900,55E-2);
            //ak.Ey_line.Add(1000,62E-2);

            return ak;
        }
    }

}
