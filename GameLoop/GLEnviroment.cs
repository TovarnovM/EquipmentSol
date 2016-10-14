using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace GameLoop {
    public class GLEnviroment {
        public List<IUnit> Units { get; private set; } = new List<IUnit>();
        public Dictionary<string,object> Stats { get; set; } = new Dictionary<string,object>();
        public double Time { get; set; }
        public double dT { get; set; } = 0.01;
        public Func<bool> StopFunc { get; set; }
        public double MaxTime { get; set; } = 100d;

        public GLEnviroment() {
            StopFunc += () => {
                return Time >= MaxTime;
            };
        }
        public void AddUnit(IUnit unit) {
            Units.Add(unit);
            unit.Owner = this;
        }
        public void UpdateAllUnits() {
            foreach(var unit in Units.Where(u=>u.Enabled)) {
                unit.Update();
            }
        }
        public void EnableAllUnits() {
            Units.ForEach(u => u.Enabled = true);
        }
        public IEnumerable<T> GetUnitsSpec<T>(bool enabletMatters = true) {
            return Units.Where(u => (!enabletMatters || u.Enabled) && u is T).Cast<T>();
        }
        public void Start() {
            Time = 0d;
            while(true) {
                UpdateAllUnits();
                if(StopFunc())
                    break;
                Time += dT;
            }
        }

        public static void SaveToXmlFile<T>(T saveMe, string filePath) {
            try {
                XmlSerializer serial = new XmlSerializer(typeof(T));
                var sw = new StreamWriter(filePath);
                serial.Serialize(sw,saveMe);
                sw.Close();
            }
            catch(Exception) { }
        }

        public static T LoadFromXmlFile<T>(string filePath) {
            try {
                XmlSerializer serial = new XmlSerializer(typeof(T));
                var sw = new StreamReader(filePath);
                T result = (T)serial.Deserialize(sw);
                sw.Close();
                return result;
            }
            catch(Exception) { }
            return default(T);
        }
    }
}
