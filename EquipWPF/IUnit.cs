using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipWPF {
    public interface IUnit {
        double UnitTime { get; set; }
        Enviroment Owner { get; set; }
        string Name { get; set; }
        void Update();
        bool Enabled { get; set; }
        Vector Pos { get; }
    }
}
