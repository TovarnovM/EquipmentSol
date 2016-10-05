using MyRandomGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipWPF {
    public abstract class UnitBase : IUnit {
        #region IUnit interface impl
        public double UnitTime { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public Enviroment Owner { get; set; }
        public Vector Pos {
            get {
                return _pos;
            }
            set {
                _pos = value;
            }
        }
        public abstract void Update();
        #endregion
        protected MyRandom rnd;

        public UnitBase(string name) {
            rnd = new MyRandom();
            Name = name;
        }

        protected Vector _pos;
        public double X {
            get {
                return _pos.X;
            }
            set {
                _pos.X = value;
            }
        }
        public double Y {
            get {
                return _pos.Y;
            }
            set {
                _pos.Y = value;
            }
        }

        public double getDistanceTo(IUnit tome) {
            return (_pos - tome.Pos).Length;
        }
    }
}
