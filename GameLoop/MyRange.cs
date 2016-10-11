using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLoop {
    public struct MyRange<T> where T : IComparable {
        public T Min;
        public T Max;
        public MyRange(T min, T max) {
            bool minmax = min.CompareTo(max) < 0;
            Min = minmax ? min : max;
            Max = minmax ? max : min;
        }
        public bool inRange(T val) {
            return val.CompareTo(Min) >= 0 && val.CompareTo(Max) < 0;
        }
    }
}
