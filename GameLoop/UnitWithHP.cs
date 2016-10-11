using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameLoop {
    public class UnitWithHP : UnitWithPos, ILiveUnit {
        public Dictionary<string,List<Tuple<double,double>>> Hitters { get; set; } = new Dictionary<string,List<Tuple<double,double>>>();
        public double HP { get; set; }
        public bool Dead { get; set; }
        public UnitWithHP():base(nameof(UnitWithHP)) {

        }
        public UnitWithHP(string name) : base(name) {
        }

        public override void Update() {
            Dead = HP <= 0;
            base.Update();
        }

        public void HitMe(IUnit from,double damage) {
            if(Hitters.ContainsKey(from.Name))
                Hitters[from.Name].Add(new Tuple<double, double>(Owner?.Time ?? UnitTime,damage));
            else {
                Hitters.Add(from.Name,new List<Tuple<double, double>>());
                HitMe(from,damage);
                return;
            }
                
            HP -= damage;
                    
        }
    }
}
