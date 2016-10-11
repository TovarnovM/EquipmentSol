using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyRandomGenerator;
using System.Xml.Serialization;
using GameLoop;

namespace EquipWPF {
    [Serializable]
    public class Fighter : UnitWithHP, ISurfaceUnit {
        //E = Ex*dist(m) [m]
        public double Ex { get; set; }
        public double Ey { get; set; }
        public IAimSurface AimSurf { get; set; }
        public List<Tuple<double,Vector>> Hits { get; set; } = new List<Tuple<double,Vector>>();
        [XmlIgnore]
        public IWeapon Weapon { get; set; }
        private string weaponName;

        public string WeaponName {
            get { return weaponName; }
            set {                
                WeaponFactory.Get(value);
                weaponName = Weapon.Name;
            }
        }

        [XmlIgnore]
        public ISurfaceUnit Target { get; set; }
        public double TimeToNextLine { get; set; }

        public void HitMe(IUnit from,IWeapon byWeapon,Vector hit ) {
            var posU = from as IWithPos<Vector>;
            var dist = posU != null ? posU.GetDistanceTo(Pos) : 10d;
            Hits.Add(new Tuple<double,Vector>(UnitTime,hit));
            HitMe(from,AimSurf.getDamage(hit) * byWeapon.GetDamage(dist));

        }
        public void Reload() {
            Weapon.RoundInMagaz = Weapon.RoundInMagazMax;
            TimeToNextLine = rnd.GetDouble(Weapon.TimeBetweenLines.Min,Weapon.TimeBetweenLines.Min) +
                             rnd.GetDouble(Weapon.ReloadTime.Min,Weapon.ReloadTime.Min);
        }
        public void Shoot() {
            int hitsInLine = rnd.GetInt(Weapon.RoundInLine.Min,Weapon.RoundInLine.Max + 1);
            hitsInLine = Math.Min(Weapon.RoundInMagaz, hitsInLine);

            var posU = Target as IWithPos<Vector>;
            var dist = posU != null ? posU.GetDistanceTo(Pos) : 10d;

            var stp = new Vector(
                rnd.GetNorm(Target.AimSurf.AimPoint.X,Ex * dist / 0.674),
                rnd.GetNorm(Target.AimSurf.AimPoint.Y,Ey * dist / 0.674));

            for(int i = 0; i < hitsInLine; i++) {
                var hit = stp + new Vector(
                    rnd.GetNorm(0,Weapon.Ex_line[dist] / 0.674),
                    rnd.GetNorm(0,Weapon.Ey_line[dist] / 0.674));
                Target.HitMe(this,Weapon,hit);
            }
            Weapon.RoundInMagaz -= hitsInLine;

            TimeToNextLine = rnd.GetDouble(Weapon.TimeBetweenLines.Min,Weapon.TimeBetweenLines.Min);

        }

        public override void Update() {
            TimeToNextLine -= DeltaT;
            if(TimeToNextLine < 0) {
                Shoot();
                if(Weapon.RoundInMagaz <= 0)
                    Reload();
            } 
            base.Update();
        }
        public Fighter():this(nameof(Fighter)) {

        }

        public Fighter(string name):base(name) {
        }
    }
}
