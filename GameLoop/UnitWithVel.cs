using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameLoop {
    public class UnitWithVel: UnitWithPos, IWithVel<Vector> {
        public UnitWithVel():base(nameof(UnitWithVel)) {

        }

        public UnitWithVel(string name) : base(name) {
        }

        public Vector Vel { get; set; }
        public override void Update() {
            Pos += Vel * (Owner.Time - UnitTime);

            base.Update();
        }
    }
}
