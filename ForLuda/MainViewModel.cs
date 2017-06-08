using EquipWPF;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForLuda {
    public class MainViewModel_1 {
        public MainViewModel_1() {
            Model1 = GetNewModel("Цель");
            Model1.PlotAreaBackground = OxyColors.LightGray;
        }
        public PlotModel Model1 { get; set; }
        public void DrawAim(PlotModel pm, Tank f, int nShow, int nVsego, double p) {
            pm.Title = $"Цель \"{f.Name}\", N = {nVsego} выстрелов (показано {nShow}), вероятность поражения P = {p}";


            pm.Series.Clear();
            var ss = new ScatterSeries() {
                MarkerType = MarkerType.Star,
                MarkerSize = 4,
                MarkerFill = OxyColors.Red,
                MarkerStroke = OxyColors.Red,
                MarkerStrokeThickness = 1,


            };
            foreach (var point in f.Hits.Take(nShow)) {
                var sp = new ScatterPoint(point.Item2.X, point.Item2.Y, value: point.Item1, tag: "t = " + point.Item1.ToString("G1"));

                ss.Points.Add(sp);
            }
            pm.Series.Add(ss);

            pm.Annotations.Clear();

            foreach (var box in f.AimSurf.Boxes) {
                var ra = new RectangleAnnotation() {
                    MinimumX = box.xmin,
                    MaximumX = box.xmax,
                    MinimumY = box.ymin,
                    MaximumY = box.ymax,
                    Fill = OxyColors.Blue.ChangeSaturation(box.damage),
                    Layer = AnnotationLayer.BelowSeries
                };
                pm.Annotations.Add(ra);
            }

            var ap = new PointAnnotation() {
                X = f.AimSurf.AimPoint.X,
                Y = f.AimSurf.AimPoint.Y,
                Shape = MarkerType.Plus,
                Stroke = OxyColors.Green,
                StrokeThickness = 3,
                Size = 7,
                Text = $"({f.AimSurf.AimPoint})",
                Layer = AnnotationLayer.AboveSeries
            };

            pm.Annotations.Add(ap);


            pm.InvalidatePlot(true);



        }

        public PlotModel GetNewModel(string title = "") {
            var m = new PlotModel { Title = title };
            var linearAxis1 = new LinearAxis();
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MaximumPadding = 0;
            linearAxis1.MinimumPadding = 0;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            linearAxis1.Position = AxisPosition.Bottom;
            m.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MaximumPadding = 0;
            linearAxis2.MinimumPadding = 0;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            m.Axes.Add(linearAxis2);

            m.PlotType = PlotType.Cartesian;
            return m;
        }
    }
}
