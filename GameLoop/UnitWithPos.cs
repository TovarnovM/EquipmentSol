using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameLoop {
    public class UnitWithPos : UnitBase, IWithPos<Vector> {
        public Vector Pos { get; set; }

        public double GetDistanceTo(Vector tome) {
            return (Pos - tome).Length;
        }

        public UnitWithPos():base(nameof(UnitWithPos)) {

        }
        public UnitWithPos(string name) : base(name) {
        }
    }
}
