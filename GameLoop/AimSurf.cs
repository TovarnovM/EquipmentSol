using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Globalization;
using System.Xml.Serialization;

namespace GameLoop {
    [Serializable]
    public class Rect {
        public double xmin, ymin, xmax, ymax, damage;
        [XmlIgnore]
        public double Width {
            get {
                return xmax - xmin;
            }
            set {
                if(value > 0)
                    xmax = xmin + value;
                else
                    xmin = xmax + value;
            }
        }
        [XmlIgnore]
        public double Height {
            get {
                return ymax - ymin;
            }
            set {
                if(value > 0)
                    ymax = ymin + value;
                else
                    ymin = ymax + value;
            }
        }
        public Rect() : this(0,0,1,1,0) { }
        public Rect(double xmin,double ymin,double xmax,double ymax,double damage) {
            this.xmin = Math.Min(xmin,xmax);
            this.ymin = Math.Min(ymin,ymax);
            this.xmax = Math.Max(xmin,xmax);
            this.ymax = Math.Max(ymin,ymax);
            this.damage = damage;//0-1
        }
        public double isHit(Vector point) {
            return isHit(point.X,point.Y);
        }
        public double isHit(double hX,double hY) {
            if((xmin < hX) && (hX < xmax)) {
                if((ymin < hY) && (hY < ymax)) {
                    return damage;
                }
            }
            return 0;
        }
        public Rect CopyMe() {
            return new Rect(xmin,ymin,xmax,ymax,damage);
        }
    }
    [Serializable]
    public class AimSurf : IAimSurface {
        
        public Vector AimPoint { get; set; }

        public IList<Rect> Boxes { get; set; } = new List<Rect>();
        //List<Vector> hits = new List<Vector>();

        public AimSurf() {
            //
        }
        
        public void AddBox(double xmin,double ymin,double xmax,double ymax,double damage) {
            Boxes.Add(new Rect(xmin,ymin,xmax,ymax,damage));
        }
        public double getDamage(double x, double y) {
            for(int i = Boxes.Count - 1; i >= 0; i--) {
                if (Boxes[i].isHit(x,y) != 0) {
                    return Boxes[i].isHit(x,y);
                }
            }
            return 0;        
        }
        public double getDamage(Vector hit) {
            return getDamage(hit.X,hit.Y);

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
                    Boxes.Add(new Rect(outt[0], outt[1], outt[2], outt[3], outt[4]));
                    Console.Write("\n");
                }
            }
        }
        public void writeInCSV(String filename) {
            //???

        }

        public IAimSurface CopyMe() {
            var res = new AimSurf();
            res.AimPoint = AimPoint;
            foreach(var box in Boxes) {
                res.Boxes.Add(box.CopyMe());
            }
            return res;
        }
    }
}
