using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipWPF {
    public interface IWeapon {
        int RoundInMagazMax { get; set; }
        int RoundInMagaz { get; set; }
        MyRange<int> RoundInLine { get; set; }
        MyRange<double> TimeBetweenLines { get; set; }
        MyRange<double> ReloadTime { get; set; }
        Func<double,double> GetDamage { get; }
        double Ex_line { get; set; }
        double Ey_line { get; set; }
    }

    public static class WeaponFactory {
        private class WeaponBase : IWeapon {
            public int RoundInMagazMax { get; set; }
            public int RoundInMagaz { get; set; }
            public MyRange<int> RoundInLine { get; set; }
            public MyRange<double> TimeBetweenLines { get; set; }
            public MyRange<double> ReloadTime { get; set; }
            public Func<double,double> GetDamage { get; set; }
            public double Ex_line { get; set; }
            public double Ey_line { get; set; }
        }
        public static IWeapon GetNewAK47() {
            return new WeaponBase() {
                RoundInMagazMax = 30,
                RoundInMagaz = 30,
                RoundInLine = new MyRange<int>(2,4),
                TimeBetweenLines = new MyRange<double>(2,3),
                ReloadTime = new MyRange<double>(4,6),
                Ex_line = 1d / 300d,
                Ey_line = 1d / 400d,
                GetDamage = (dist) => {
                    return dist < 1500 ? 1 : (dist < 3000 ? 0.5 : 0);
                }
            };
        }
    }

}
