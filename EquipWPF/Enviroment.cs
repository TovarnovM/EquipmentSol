using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipWPF {
    public class Enviroment {
        public List<IUnit> Units { get; private set; } = new List<IUnit>();
        public double Time { get; set; }
        public double dT { get; set; } = 0.0001;
        public Func<bool> StopFunc { get; set; }
        public double MaxTime { get; set; } = 100d;

        public Enviroment() {
            StopFunc += () => {
                return Time >= MaxTime;
            };
        }
        public void AddUnit(IUnit unit) {
            Units.Add(unit);
            unit.Owner = this;
        }
        public void UpdateAllUnits() {
            foreach(var unit in Units.Where(u=>u.Enabled)) {
                unit.Update();
            }
        }
        public void EnableAllUnits() {
            Units.ForEach(u => u.Enabled = true);
        }
        public void Start() {
            Time = 0d;
            while(true) {
                UpdateAllUnits();
                if(StopFunc())
                    break;
                Time += dT;
            }
        }
    }
}
