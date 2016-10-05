using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipWPF {
    public class UnitWithHPandGun : UnitBase {
        public IWeapon Weapon { get; set; }
        public double HP { get; set; }
        public bool Dead {
            get {
                return HP <= 0;
            }
        }
        public UnitWithHPandGun(string name, IWeapon weapon) : base(name) {
            Weapon = weapon;
        }

        public override void Update() {
            throw new NotImplementedException();
        }
    }
}
