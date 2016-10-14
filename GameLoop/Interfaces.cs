using Interpolator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameLoop {
    public interface IAimSurface {
        IList<Rect> Boxes { get; }
        double getDamage(Vector hit);
        Vector AimPoint { get; set; }
        IAimSurface CopyMe();
    }
    public interface IUnit {
        double UnitTime { get; set; }
        double DeltaT { get; }
        GLEnviroment Owner { get; set; }
        string Name { get; set; }
        void Update();
        bool Enabled { get; set; }
    }
    public interface IWeapon {
        string Name { get; }
        int RoundInMagazMax { get; set; }
        int RoundInMagaz { get; set; }
        MyRange<int> RoundInLine { get; set; }
        MyRange<double> TimeBetweenLines { get; set; }
        MyRange<double> ReloadTime { get; set; }
        Func<double,double> GetDamage { get; }
        double Ex_line(double distance);
        double Ey_line(double distance);
    }

    public interface IWithPos<T> {
        T Pos { get; set; }
        double GetDistanceTo(T tome);
    }
    public interface IWithVel<T>: IWithPos<T> {
        T Vel { get; set; }
    }
    public interface ILiveUnit {
        double HP { get; set; }
        bool Dead { get; set; }
        void HitMe(IUnit from, double damage);       
    }
    public interface ISurfaceUnit:ILiveUnit {
        IAimSurface AimSurf { get; }
        void HitMe(IUnit from, IWeapon byWeapon, Vector hit);
    }
}
