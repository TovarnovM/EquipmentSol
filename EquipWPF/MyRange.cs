using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipWPF {
    public struct MyRange<T> {
        public T Min;
        public T Max;
        public MyRange(T min, T max) {
            Min = max;
            Max = max;
        }
    }
}
