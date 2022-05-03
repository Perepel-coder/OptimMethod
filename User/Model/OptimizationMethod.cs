using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    public enum Quality { BEST, WORST, NEUTRAL}
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double FunctionValue { get; set; }
        public bool Complex { get; set; } = false;
        public Quality Quality { get; set; } = Quality.NEUTRAL;
    }
    // для функции 2-х переменных
    internal class ComplexBoxingMethod
    {

        private int N;  // число точек комплекса
        private Point pointFbest;   // лучшее значение функции
        private Point pointFworst;  // худшее значенеи функции
        private Point centr;    // центр комплекса
        private double xmin;    // нижнее ограничение по х
        private double xmax;    // верхнее ограничение по x
        private double ymin;    // нижнее ограничение по y
        private double ymax;    // верхнее ограничение по y
        private double k;       // k: y=kx+b
        private double b;       // b: y=kx+b
        private string sing;    // знак ограничения второго рода

        //-----------------временно-------------------------
        private double ε = 0.01;
        //------------------------------------------
        #region конструктор
        public ComplexBoxingMethod( int n, double k, double b, string sing,
            double xmin, double xmax, double ymin, double ymax)
        {
            this.N = 2 * n;
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymin = ymin;
            this.ymax = ymax;
            this.k = k;
            this.b = b;
            this.sing = sing;
            pointFbest = new();
            pointFworst = new();
            centr = new();
        }
        #endregion

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
        private List<Point> GetComplex()     // 1) Формирование исходного Комплекса
        {
            Random rnd = new();
            List<Point> points = new();
            int numberCorrectPoints = 0;
            double sumCorrectX = 0;
            double sumCorrectY = 0;
            for (int i = 0; i < N; i++)
            {
                double x = Math.Round(this.xmin + rnd.NextDouble() * (this.xmax - this.xmin), 2);
                double y = Math.Round(this.ymin + rnd.NextDouble() * (this.ymax - this.ymin), 2);
                bool flag = CheckConditionSecondKind(x, y, this.sing);
                if (flag)
                {
                    numberCorrectPoints++;
                    sumCorrectX += x;
                    sumCorrectY += y;
                }
                points.Add(new Point { X = x, Y = y, Complex = flag });  
            }
            for (int i = 0; i < points.Count; i++)
            {
                while (!points[i].Complex)
                {
                    points[i].X = Math.Round((sumCorrectX / numberCorrectPoints + points[i].X) / 2, 2);
                    points[i].Y = Math.Round((sumCorrectY / numberCorrectPoints + points[i].Y) / 2, 2);
                    points[i].Complex = CheckConditionSecondKind(points[i].X, points[i].Y, this.sing);
                }
                numberCorrectPoints++;
                sumCorrectX += points[i].X;
                sumCorrectY += points[i].Y;
            }
            return points;
        }
        private void GetValuesFunction(List<Point> points)    // 2) Вычисление значений целевой функции.
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i].FunctionValue = GetTask15(points[i]);
            }
        }
        private void GetBestAndWorstValue(List<Point> points, bool max)    // 3) Выбор наилучшего и наихудшего значения
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (i == 0) { continue; }
                if (points[i].FunctionValue <= points[i - 1].FunctionValue)
                {
                    points[i - 1].Quality = Quality.NEUTRAL;
                    if (max) { points[i].Quality = Quality.WORST; }
                    else { points[i].Quality = Quality.BEST; }  
                }
                if (points[i].FunctionValue >= points[i - 1].FunctionValue)
                {
                    points[i - 1].Quality = Quality.NEUTRAL;
                    points[i].Quality = Quality.BEST;
                    if (max) { points[i].Quality = Quality.BEST; }
                    else { points[i].Quality = Quality.WORST; }
                }
            }
            this.pointFbest = points.Where(x => x.Quality == Quality.BEST).Select(el => el).Single();
            this.pointFworst = points.Where(x => x.Quality == Quality.WORST).Select(el => el).Single();
        }
        private bool GetCoordCenter(List<Point> points, double ε)    // 4) Определение координат Сi центра Комплекса. 5) Проверка условия окончания поиска
        {
            this.centr.X = Math.Pow(N - 1, -1) * (points.Sum(el => el.X) - pointFworst.X);
            this.centr.Y = Math.Pow(N - 1, -1) * (points.Sum(el => el.Y) - pointFworst.Y);
            double sum = (Math.Abs(centr.X - pointFworst.X) + Math.Abs(centr.X - pointFbest.X)) +
                (Math.Abs(centr.Y - pointFworst.Y) + Math.Abs(centr.Y - pointFbest.Y));
            double B = Math.Pow(2 * N, -1) * sum;
            return B < ε ? true : false;
        }
        private void GetPointInsteadWorstPoint(List<Point> points, double ε) // 6) - 10) 
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Quality == Quality.WORST)
                {
                    // 6) Вычисление координаты новой точки Комплекса взамен наихудшей
                    points[i].Complex = false;
                    points[i].X = 2.3 * this.centr.X - 1.3 * this.pointFbest.X;
                    points[i].Y = 2.3 * this.centr.Y - 1.3 * this.pointFbest.Y;

                    if (points[i].X <= xmin) { points[i].X = xmin + ε; }
                    if (points[i].X >= xmax) { points[i].X = xmax - ε; }
                    if (points[i].Y <= ymin) { points[i].X = ymin + ε; }
                    if (points[i].Y >= ymax) { points[i].X = ymax - ε; }

                    // 7) Проверка выполнения ограничений 2.го рода для новой точки.
                    while (!points[i].Complex)
                    {
                        points[i].Complex = CheckConditionSecondKind(points[i].X, points[i].Y, sing);
                        points[i].X = (1 / 2) * (points[i].X + centr.X);
                        points[i].Y = (1 / 2) * (points[i].Y + centr.Y);
                    }
                    // 8) Вычисление значения целевой функции F0 в новой точке
                    points[i].FunctionValue = GetTask15(points[i]);

                    // 9) Нахождение новой вершины смещением xi0   на половину расстояния к лучшей из вершин комплекса
                    // 10) Замена наихудшей точки
                    while (points[i].FunctionValue < pointFworst.FunctionValue)
                    {
                        points[i].X = (1 / 2) * (points[i].X + pointFbest.X);
                        points[i].Y = (1 / 2) * (points[i].Y + pointFbest.Y);
                        points[i].FunctionValue = GetTask15(points[i]);
                    }
                    break;
                }
            }
        }
        private double GetTask15(Point point)      // вычисление значений целевой функции (вариант 15)
        {
            double a = 1, β = 1, y = 3.14, p1 = 1, p2 = 1, N = 2;
            double sqrt = Math.Sqrt(Math.Pow(point.X, N) + Math.Pow(point.Y, N));
            double FunctionValue = a * (point.X - β * p1) * Math.Cos(y * p2 * sqrt);
            return FunctionValue;
        }
        private double GetTask18(Point point)      // вычисление значений целевой функции (вариант 18)
        {
            double a = 1, β = 1, μ = 1, A = 1, G = 2, N = 2;
            double FunctionValue = a * (G * μ * (Math.Pow(point.Y - point.X, N) + Math.Pow(β * A - point.X, N)));
            return FunctionValue;
        }

        public void Solve(bool max)
        {
            List<Point> points = GetComplex();
            GetValuesFunction(points);
            GetBestAndWorstValue(points, max);
            bool end = GetCoordCenter(points, this.ε);
            while (!end)
            {
                GetPointInsteadWorstPoint(points, ε);
                GetBestAndWorstValue(points, max);
                end = GetCoordCenter(points, this.ε);
            }
        }
    }
}
