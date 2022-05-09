using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace User.Model
{
    public enum Quality { BEST, WORST, NEUTRAL}
    public  delegate double task(Point2 point);
    public class Point2
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double FunctionValue { get; set; }
        public bool Complex { get; set; } = false;
        public Quality Quality { get; set; } = Quality.NEUTRAL;
    }
    public class Point3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    internal interface IMethod
    {
        public string? Name { get; }
        public Point2 Solve();
        public void RegisterMethod(bool max, double k, double b, string sing,
            double xmin, double xmax, double ymin, double ymax, double ε, task task);
        public ObservableCollection<Point3> GetChartData();
        public ObservableCollection<Point2> GetChartLimitationData();
        public List<List<Point3>> GetChartDataAsTable();
    }

    // для функции 2-х переменных метод Бокса
    internal class ComplexBoxingMethod: IMethod
    {
        public string? Name { get; } = "Комплекс-метод Бокса";
        private int N;               // число точек комплекса
        private Point2? pointFbest;   // лучшее значение функции
        private Point2? pointFworst;  // худшее значенеи функции
        private Point2? centr;        // центр комплекса
        private double xmin;         // нижнее ограничение по х
        private double xmax;         // верхнее ограничение по x
        private double ymin;         // нижнее ограничение по y
        private double ymax;         // верхнее ограничение по y
        private double k;            // k: y=kx+b
        private double b;            // b: y=kx+b
        private string? sing;         // знак ограничения второго рода
        private double ε;            // точность
        private bool max;            // тип экстремума
        private task task;           // задача
        //------------------------------------------
        private bool CheckConditionSecondKind(double x, double y, string sing)  // проверить выполнение условия 2-ого рода
        {
            bool flag = false;
            switch (sing)
            {
                case "⩽": flag = (y <= k * x + b) ? true : false; break;
                case "⩾": flag = (y >= k * x + b) ? true : false; break;
                case ">": flag = (y > k * x + b) ? true : false; break;
                case "<": flag = (y < k * x + b) ? true : false; break;
            }
            return flag;
        }
        private List<Point2> GetComplex()     // 1) Формирование исходного Комплекса
        {
            Random rnd = new();
            List<Point2> points = new();
            int numberCorrectPoints = 0;
            double sumCorrectX = 0;
            double sumCorrectY = 0;
            bool flag = false;
            while (!flag)
            {
                double x = this.xmin + (rnd.Next(1, 999) / 1000.0f) * (this.xmax - this.xmin);
                double y = this.ymin + (rnd.Next(1, 999) / 1000.0f) * (this.ymax - this.ymin);
                flag = CheckConditionSecondKind(x, y, this.sing);
                if (flag)
                {
                    numberCorrectPoints++;
                    sumCorrectX += x;
                    sumCorrectY += y;
                    points.Add(new Point2 { X = x, Y = y, Complex = flag });
                } 
            }
            for (int i = 1; i < this.N; i++)
            {
                double x = this.xmin + (rnd.Next(1, 999) / 1000.0f) * (this.xmax - this.xmin);
                double y = this.ymin + (rnd.Next(1, 999) / 1000.0f) * (this.ymax - this.ymin);
                flag = CheckConditionSecondKind(x, y, this.sing);
                if (flag)
                {
                    numberCorrectPoints++;
                    sumCorrectX += x;
                    sumCorrectY += y;
                }
                points.Add(new Point2 { X = x, Y = y, Complex = flag });  
            }
            for (int i = 0; i < points.Count; i++)
            {
                flag = false;
                while (!points[i].Complex)
                {
                    flag = true;
                    points[i].Complex = CheckConditionSecondKind(points[i].X, points[i].Y, this.sing);
                    if (points[i].Complex) { break; }
                    points[i].X = (sumCorrectX / numberCorrectPoints + points[i].X) / 2.0f;
                    points[i].Y = (sumCorrectY / numberCorrectPoints + points[i].Y) / 2.0f;
                }
                if (flag)
                {
                    numberCorrectPoints++;
                    sumCorrectX += points[i].X;
                    sumCorrectY += points[i].Y;
                }
            }
            return points;
        }
        private void GetValuesFunction(List<Point2> points)    // 2) Вычисление значений целевой функции.
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i].FunctionValue = task(points[i]);
            }
        }
        private void GetBestAndWorstValue(List<Point2> points)    // 3) Выбор наилучшего и наихудшего значения
        {
            double maxV = Double.MinValue; int maxVid = 0;
            double minV = Double.MaxValue; int minVid = 0;
            foreach (var point in points) { point.Quality = Quality.NEUTRAL; }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].FunctionValue >= maxV) { maxV = points[i].FunctionValue; maxVid = i; }
                if (points[i].FunctionValue <= minV) { minV = points[i].FunctionValue; minVid = i; }
            }
            if (max)
            {
                if (maxVid == minVid) 
                {
                    maxVid = 0;
                    minVid = 1;
                }
                points[maxVid].Quality = Quality.BEST;
                points[minVid].Quality = Quality.WORST;

                this.pointFbest = new Point2
                {
                    X = points[maxVid].X,
                    Y = points[maxVid].Y,
                    Complex = points[maxVid].Complex,
                    FunctionValue = points[maxVid].FunctionValue,
                    Quality = points[maxVid].Quality
                };
                this.pointFworst = new Point2
                {
                    X = points[minVid].X,
                    Y = points[minVid].Y,
                    Complex = points[minVid].Complex,
                    FunctionValue = points[minVid].FunctionValue,
                    Quality = points[minVid].Quality
                };
            }
            else
            {
                if (maxVid == minVid)
                {
                    maxVid = 0;
                    minVid = 1;
                }
                points[maxVid].Quality = Quality.WORST;
                points[minVid].Quality = Quality.BEST;

                this.pointFworst = new Point2
                {
                    X = points[maxVid].X,
                    Y = points[maxVid].Y,
                    Complex = points[maxVid].Complex,
                    FunctionValue = points[maxVid].FunctionValue,
                    Quality = points[maxVid].Quality
                };
                this.pointFbest = new Point2
                {
                    X = points[minVid].X,
                    Y = points[minVid].Y,
                    Complex = points[minVid].Complex,
                    FunctionValue = points[minVid].FunctionValue,
                    Quality = points[minVid].Quality
                };
            }
        }
        private bool GetCoordCenter(List<Point2> points, double ε)    // 4) Определение координат Сi центра Комплекса. 5) Проверка условия окончания поиска
        {
            this.centr.X = (points.Where(x => x.Quality != Quality.WORST).Sum(el => el.X)) / (N - 1);
            this.centr.Y = (points.Where(x => x.Quality != Quality.WORST).Sum(el => el.Y)) / (N - 1);

            double cross1 = Math.Sqrt(Math.Pow(centr.X - pointFworst.X, 2) + Math.Pow(centr.Y - pointFworst.Y, 2));
            double cross2 = Math.Sqrt(Math.Pow(centr.X - pointFbest.X, 2) + Math.Pow(centr.Y - pointFbest.Y, 2));
            double B = cross1 + cross2;
            return (B < ε) ? true : false;
        }
        private void GetPointInsteadWorstPoint(bool max, List<Point2> points, double ε) // 6) - 10) 
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Quality == Quality.WORST)
                {
                    // 6) Вычисление координаты новой точки Комплекса взамен наихудшей
                    points[i].Complex = false;
                    points[i].X = 2.3f * this.centr.X - 1.3f * this.pointFworst.X;
                    points[i].Y = 2.3f * this.centr.Y - 1.3f * this.pointFworst.Y;

                    if (points[i].X < xmin) { points[i].X = xmin + ε; }
                    if (points[i].X > xmax) { points[i].X = xmax - ε; }
                    if (points[i].Y < ymin) { points[i].Y = ymin + ε; }
                    if (points[i].Y > ymax) { points[i].Y = ymax - ε; }

                    // 7) Проверка выполнения ограничений 2.го рода для новой точки.
                    while (true)
                    {
                        points[i].Complex = CheckConditionSecondKind(points[i].X, points[i].Y, sing);
                        if (points[i].Complex) { break; }
                        points[i].X = (points[i].X + centr.X) / 2.0f;
                        points[i].Y = (points[i].Y + centr.Y) / 2.0f;
                    }
                    // 8) Вычисление значения целевой функции F0 в новой точке
                    points[i].FunctionValue = task(points[i]);

                    // 9) Нахождение новой вершины смещением xi0   на половину расстояния к лучшей из вершин комплекса
                    // 10) Замена наихудшей точки
                    if (max)
                    {
                        while (points[i].FunctionValue <= pointFworst.FunctionValue)
                        {
                            points[i].X = (points[i].X + pointFbest.X) / 2.0f;
                            points[i].Y = (points[i].Y + pointFbest.Y) / 2.0f;
                            points[i].FunctionValue = task(points[i]);
                        }
                    }
                    else
                    {
                        while (points[i].FunctionValue >= pointFworst.FunctionValue)
                        {
                            points[i].X = (points[i].X + pointFbest.X) / 2.0f;
                            points[i].Y = (points[i].Y + pointFbest.Y) / 2.0f;
                            points[i].FunctionValue = task(points[i]);
                        }
                    }
                    break;
                }
            }
        }
       
        public Point2 Solve()
        {
            List<Point2> points = GetComplex();
            bool end;
            GetValuesFunction(points);
            do
            {
                GetBestAndWorstValue(points);
                if (pointFbest == pointFworst) { return points.Where(x => x.Quality == Quality.BEST).Select(el => el).Single(); }
                end = GetCoordCenter(points, this.ε);
                if (end) { return points.Where(x=>x.Quality == Quality.BEST).Select(el=>el).Single(); }
                GetPointInsteadWorstPoint(true, points, this.ε);
            }
            while (true);
        }
        public void RegisterMethod(bool max, double k, double b, string sing,
            double xmin, double xmax, double ymin, double ymax, double ε, task task)
        {
            this.pointFbest = new();
            this.pointFworst = new();
            this.centr = new();
            this.N = 200;
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymin = ymin;
            this.ymax = ymax;
            this.max = max;
            this.k = k;
            this.b = b;
            this.sing = sing;
            this.ε = ε;
            this.task = task;
        }
        public ObservableCollection<Point3> GetChartData()
        {
            float step = 0.1f;
            ObservableCollection<Point3> chart3dData = new();
            for (float i = ((float)xmin); i < xmax; i += step) 
            {
                for (float j = ((float)ymin); j < ymax; j += step)
                {
                    chart3dData.Add(new Point3 { X = i, Y = j, Z = (float)task(new Point2 { X = i, Y = j}) });
                }
            }
            return chart3dData;
        }
        public List<List<Point3>> GetChartDataAsTable()
        {
            float step = 0.5f;
            List<List<Point3>> data = new(); int row = 0;
            for (float i = ((float)xmin); i < xmax; i += step, row++)
            {
                data.Add(new());
                for (float j = ((float)ymin); j < ymax; j += step)
                {
                    data[row].Add(new Point3 { X = i, Y = j, Z = (float)task(new Point2 { X = i, Y = j }) });
                }
            }
            return data;
        }
        public ObservableCollection<Point2> GetChartLimitationData()
        {
            ObservableCollection<Point2> limitationData = new();
            ObservableCollection<Point2> points = new();
            double yXmin = k * xmin + b;
            double yXmax = k * xmax + b;
            double xYmin = (ymin - b) / k;
            double xYmax = (ymax - b) / k;

            if (ymin <= yXmax && yXmax <= ymax)
            {
                limitationData.Add(new Point2 { X = xmax, Y = yXmax});
            }
            if (xmin <= xYmax && xYmax <= xmax)
            {
                limitationData.Add(new Point2 { X = xYmax, Y = ymax});
            }
            if (ymin <= yXmin && yXmin <= ymax) 
            {
                limitationData.Add(new Point2 { X = xmin, Y = yXmin });
            }
            if (xmin <= xYmin && xYmin <= xmax)
            {
                limitationData.Add(new Point2 { X = xYmin, Y = ymin });
            }
            return limitationData;
        }
    }
    internal static class Methodlist
    {
        public static List<IMethod> methods = new()
        {
            new ComplexBoxingMethod()
        };
    }
}
