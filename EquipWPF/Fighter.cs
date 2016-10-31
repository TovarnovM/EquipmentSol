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

        public double Bx { get; set; }
        public double By { get; set; }
        public IAimSurface AimSurf { get; set; }
        public List<Tuple<double,Vector>> Hits { get; set; } = new List<Tuple<double,Vector>>();
        public int Omega { get; set; } = 0;
        public double TimeBetweenLine_Min { get; set; }
        public double TimeBetweenLine_Max { get; set; }
        public double ReloadTime_Min { get; set; }
        public double ReloadTime_Max { get; set; }

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
            TimeToNextLine = rnd.GetDouble(TimeBetweenLine_Min,TimeBetweenLine_Max) +
                             rnd.GetDouble(ReloadTime_Min,ReloadTime_Max);
        }
        public void Shoot() {
            int hitsInLine = rnd.GetInt(Weapon.RoundInLine.Min,Weapon.RoundInLine.Max + 1);
            hitsInLine = Math.Min(Weapon.RoundInMagaz, hitsInLine);

            var posU = Target as IWithPos<Vector>;
            var dist = posU != null ? posU.GetDistanceTo(Pos) : 10d;

            var stp = new Vector(
                rnd.GetNorm(Target.AimSurf.AimPoint.X,Bx * dist / 0.674),
                rnd.GetNorm(Target.AimSurf.AimPoint.Y,By * dist / 0.674));

            for(int i = 0; i < hitsInLine; i++) {
                var hit = stp + new Vector(
                    rnd.GetNorm(0,Weapon.Ex_line(dist) / 0.674),
                    rnd.GetNorm(0,Weapon.Ey_line(dist) / 0.674));
                Target.HitMe(this,Weapon,hit);
            }
            Weapon.RoundInMagaz -= hitsInLine;
            Omega += hitsInLine;
            TimeToNextLine = rnd.GetDouble(TimeBetweenLine_Min,TimeBetweenLine_Max);

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

        public Fighter CopyMe(bool onlyInvariants = true) {
            var res = new Fighter(Name);
            res.Bx = Bx;
            res.By = By;
            res.AimSurf = AimSurf.CopyMe();
            res.WeaponName = WeaponName;

            res.Pos = Pos;

            res.HP = HP;
            res.Dead = Dead;

            res.TimeBetweenLine_Min = TimeBetweenLine_Min;
            res.TimeBetweenLine_Max = TimeBetweenLine_Max;
            res.ReloadTime_Min = ReloadTime_Min;
            res.ReloadTime_Max = ReloadTime_Max;


            if(!onlyInvariants) {
                res.Owner = Owner;
                res.UnitTime = UnitTime;
                res.Enabled = Enabled;

                foreach(var hitter in Hitters) {
                    res.Hitters.Add(hitter.Key,new List<Tuple<double,double>>(hitter.Value));
                }


                foreach(var hit in Hits) {
                    res.Hits.Add(new Tuple<double,Vector>(hit.Item1,hit.Item2));
                }
                res.Target = Target;
                res.TimeToNextLine = TimeToNextLine;


            }
            return res;

        }
        public void ClearStats() {
            Hits.Clear();
            Hitters.Clear();
        }

       
    }

}
