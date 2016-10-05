using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipWPF {
    public interface IAimSurface {
        double getDamage(Vector hit);
    }
}
