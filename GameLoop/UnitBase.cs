using MyRandomGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace GameLoop {
    public class UnitBase : IUnit {
        #region IUnit interface impl
        public double UnitTime { get; set; } = 0d;
        private bool _enabled;

        public bool Enabled {
            get {
                return _enabled;
            }
            set {
                _enabled = value;
                if(_enabled && Owner != null )
                    UnitTime = Owner.Time;
                else
                    return;
            }
        }
        public string Name { get; set; }
        [XmlIgnore]
        public GLEnviroment Owner { get; set; }

        public double DeltaT {
            get {
                if(Owner == null)
                    return 0d;
                return Owner.Time - UnitTime;
            }
        }

        public virtual void Update() {
            if(Owner == null)
                return;
            UnitTime = Owner.Time;
        }

        #endregion
        [XmlIgnore]
        protected MyRandom rnd;

        public UnitBase():this(nameof(UnitBase)) {

        }

        public UnitBase(string name) {
            rnd = new MyRandom();
            Name = name;
            _enabled = true;
        }

    }
}
