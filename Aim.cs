using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Globalization;

namespace ConsoleApplication2 {
    public class Aim {
        public class Rect {
            public double _x, _y, _width, _heigth, _damage;
            public Rect(double x, double y, double width, double heigth, double damage) {
                _x = x;
                _y = y;
                _width = width;
                _heigth = heigth;
                _damage = damage;//0-1
            }
            public double isHit(Vector point) {
                if ((this._x < point.X) & (point.X < this._x + this._width)) {
                    if ((this._y - this._heigth < point.Y) & (point.Y < this._y)) {
                        return this._damage;
                    }
                }
                return 0;
            }
        }
        //
        List<Rect> boxes = new List<Rect>();
        List<Vector> hits = new List<Vector>();

        public Aim() {
            //
        }
        public double getDamage(Vector hit) {
            hits.Add(hit);
            foreach (Rect box in boxes.AsEnumerable().Reverse()) {
                if (box.isHit(hit) != 0) {
                    return box.isHit(hit);
                }
            }
            return 0;
        }
        public void loadFromCSV(String Filename) {
            string[] strings = File.ReadAllLines(Filename);
            foreach (String str in strings) {
                if (!String.IsNullOrEmpty(str)) {
                    string stt = str.Trim(new char[] { '"', ';' });
                    Console.Write(stt);
                    string[] buf = stt.Split(',');
                    Double[] outt = new Double[buf.Length];
                    for(int i = 0; i < buf.Length; i++) {
                        //Double.TryParse(buf[i], out outt[i]);
                        outt[i]=double.Parse(buf[i], CultureInfo.InvariantCulture);
                    }
                    boxes.Add(new Rect(outt[0], outt[1], outt[2], outt[3], outt[4]));
                    Console.Write("\n");
                }
            }
        }
        public void writeInCSV(String filename) {
            //???

        }
    }
}
