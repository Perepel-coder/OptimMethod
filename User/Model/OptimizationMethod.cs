using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace User.Model
{
    public enum PointStatus { BEST, WORST, NEUTRAL}
    public  delegate double functionTwoParam(double x, double y);
    public class PointOfFunction
    {
        private functionTwoParam Function;
        private double x;
        private double y;
        private double functionValue;

        public double X { get { return x; } }
        public double Y { get { return y; } }
        public double FunctionValue { get { return functionValue; } }
        public void SetXY(double x, double y)
        {
            this.x = x;
            this.y = y;
            functionValue = Function(X, Y);
        }
        public void SetX(double x)
        {
            this.x = x;
            functionValue = Function(X, Y);
        }
        public void SetY(double y)
        {
            this.y = y;
            functionValue = Function(X, Y);
        }
        public PointOfFunction Round(int r)
        {
            PointOfFunction res = new(this.Function);
            res.SetXY(Math.Round(this.X, r), Math.Round(this.Y, r));
            res.functionValue = Math.Round(res.FunctionValue, r);
            return res;
        }
        public bool BelongingToComplex { get; set; } = false;
        public PointStatus PointStatus { get; set; } = PointStatus.NEUTRAL;

        public PointOfFunction(functionTwoParam Task) { this.Function = Task; }
    }
    public class VariableFunctionLimitations
    {
        public bool TypeOfExtremum { get; set; }
        public double k { get; set; }
        public double b { get; set; }
        public string? Sing { get; set; }
        public double Xmin { get; set; }
        public double Xmax { get; set; }
        public double Ymin { get; set; }
        public double Ymax { get; set; }
        public double ε { get; set; }
        public double step { get; set; }
        public double StartX { get; set; }
        public double StartY { get; set; }
        public int countComplex { get; set; }
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
        public PointOfFunction Solve();
        public void RegisterMethod(VariableFunctionLimitations functionLimitations, functionTwoParam task);
        public ObservableCollection<Point3> GetChartData();
        public ObservableCollection<Point3> GetChartLimitationData();
        public List<List<Point3>> GetChartDataAsTable();
    }

    // для функции 2-х переменных метод Бокса
    internal class ComplexBoxingMethod: IMethod
    {
        #region поля
        public string Name { get; } = "Комплекс-метод Бокса";
        private int N;                           // число точек комплекса
        private PointOfFunction? pointFbest;     // лучшее значение функции
        private PointOfFunction? pointFworst;    // худшее значенеи функции
        private PointOfFunction? centr;          // центр комплекса
        private double xmin;                     // нижнее ограничение по х
        private double xmax;                     // верхнее ограничение по x
        private double ymin;                     // нижнее ограничение по y
        private double ymax;                     // верхнее ограничение по y
        private double k;                        // k: y=kx+b
        private double b;                        // b: y=kx+b
        private string sing;                     // знак ограничения второго рода
        private double ε;                        // точность
        private double step;
        private bool max;                        // тип экстремума
        private functionTwoParam task;           // задача
        #endregion

        private bool CheckConditionSecondKind(double x, double y, string sing)  // проверить выполнение условия 2-ого рода
        {
            bool flag = false;
            switch (sing)
            {
                case "⩽": flag = (y <= k * x + b) ? true : false; break;
                case "⩾": flag = (y >= k * x + b) ? true : false; break;
            }
            return flag;
        }
        private List<PointOfFunction> GetComplex()     // 1) Формирование исходного Комплекса
        {
            Random rnd = new();
            List<PointOfFunction> points = new();
            int numberCorrectPoints = 0;
            double sumCorrectX = 0;
            double sumCorrectY = 0;
            bool flag = false;
            while (!flag)
            {
                double x = this.xmin + (rnd.Next(1, 101) / 100.0f) * (this.xmax - this.xmin);
                double y = this.ymin + (rnd.Next(1, 101) / 100.0f) * (this.ymax - this.ymin);
                flag = CheckConditionSecondKind(x, y, this.sing);
                if (flag)
                {
                    numberCorrectPoints++;
                    sumCorrectX += x;
                    sumCorrectY += y;
                    var point = new PointOfFunction(this.task);
                    point.SetXY(x, y);
                    point.BelongingToComplex = flag;
                    points.Add(point);
                } 
            }
            for (int i = 1; i < this.N; i++)
            {
                double x = this.xmin + (rnd.Next(1, 101) / 100.0f) * (this.xmax - this.xmin);
                double y = this.ymin + (rnd.Next(1, 101) / 100.0f) * (this.ymax - this.ymin);
                flag = CheckConditionSecondKind(x, y, this.sing);
                if (flag)
                {
                    numberCorrectPoints++;
                    sumCorrectX += x;
                    sumCorrectY += y;
                }
                var point = new PointOfFunction(this.task);
                point.SetXY(x, y);
                point.BelongingToComplex = flag;
                points.Add(point);  
            }
            for (int i = 0; i < points.Count; i++)
            {
                flag = false;
                while (!points[i].BelongingToComplex)
                {
                    flag = true;
                    points[i].BelongingToComplex = CheckConditionSecondKind(points[i].X, points[i].Y, this.sing);
                    if (points[i].BelongingToComplex) { break; }
                    double x = (sumCorrectX / numberCorrectPoints + points[i].X) / 2.0f;
                    double y = (sumCorrectY / numberCorrectPoints + points[i].Y) / 2.0f;
                    points[i].SetXY(x, y);
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
        private void GetBestAndWorstValue(List<PointOfFunction> points)    // 3) Выбор наилучшего и наихудшего значения
        {
            double maxV = Double.MinValue; int maxVid = 0;
            double minV = Double.MaxValue; int minVid = 0;
            foreach (var point in points) { point.PointStatus = PointStatus.NEUTRAL; }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].FunctionValue >= maxV) { maxV = points[i].FunctionValue; maxVid = i; }
                if (points[i].FunctionValue <= minV) { minV = points[i].FunctionValue; minVid = i; }
            }
            if (max)
            {
                if (maxVid == minVid) { maxVid = 0; minVid = 1; }
                points[maxVid].PointStatus = PointStatus.BEST;
                points[minVid].PointStatus = PointStatus.WORST;
                this.pointFbest.SetXY(points[maxVid].X, points[maxVid].Y);
                this.pointFbest.BelongingToComplex = points[maxVid].BelongingToComplex;
                this.pointFbest.PointStatus = points[maxVid].PointStatus;
                this.pointFworst.SetXY(points[minVid].X, points[minVid].Y);
                this.pointFworst.BelongingToComplex = points[minVid].BelongingToComplex;
                this.pointFworst.PointStatus = points[minVid].PointStatus;
            }
            else
            {
                if (maxVid == minVid) { maxVid = 0; minVid = 1; }
                points[maxVid].PointStatus = PointStatus.WORST;
                points[minVid].PointStatus = PointStatus.BEST;
                this.pointFbest.SetXY(points[minVid].X, points[minVid].Y);
                this.pointFbest.BelongingToComplex = points[minVid].BelongingToComplex;
                this.pointFbest.PointStatus = points[minVid].PointStatus;
                this.pointFworst.SetXY(points[maxVid].X, points[maxVid].Y);
                this.pointFworst.BelongingToComplex = points[maxVid].BelongingToComplex;
                this.pointFworst.PointStatus = points[maxVid].PointStatus;
            }
        }
        private bool GetCoordCenter(List<PointOfFunction> points, double ε)    // 4) Определение координат Сi центра Комплекса. 5) Проверка условия окончания поиска
        {
            double x = (points.Where(x => x.PointStatus != PointStatus.WORST).Sum(el => el.X)) / (N - 1);
            double y = (points.Where(x => x.PointStatus != PointStatus.WORST).Sum(el => el.Y)) / (N - 1);
            this.centr.SetXY(x, y);
            double cross1 = Math.Sqrt(Math.Pow(this.centr.X - this.pointFworst.X, 2) 
                + Math.Pow(this.centr.Y - this.pointFworst.Y, 2));
            double cross2 = Math.Sqrt(Math.Pow(this.centr.X - this.pointFbest.X, 2) 
                + Math.Pow(this.centr.Y - this.pointFbest.Y, 2));
            double B = cross1 + cross2;
            return (B < ε) ? true : false;
        }
        private void GetPointInsteadWorstPoint(bool max, List<PointOfFunction> points, double ε) // 6) - 10) 
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].PointStatus == PointStatus.WORST)
                {
                    // 6) Вычисление координаты новой точки Комплекса взамен наихудшей
                    points[i].BelongingToComplex = false;
                    double x = 2.3f * this.centr.X - 1.3f * this.pointFworst.X;
                    double y = 2.3f * this.centr.Y - 1.3f * this.pointFworst.Y;

                    points[i].SetXY(x, y);

                    if (points[i].X < this.xmin) { points[i].SetX(xmin + ε); }
                    if (points[i].X > this.xmax) { points[i].SetX(xmax - ε); }
                    if (points[i].Y < this.ymin) { points[i].SetY(ymin + ε); }
                    if (points[i].Y > this.ymax) { points[i].SetY(ymax - ε); }

                    // 7) Проверка выполнения ограничений 2.го рода для новой точки.
                    while (true)
                    {
                        points[i].BelongingToComplex = CheckConditionSecondKind(points[i].X, points[i].Y, this.sing);
                        if (points[i].BelongingToComplex) { break; }
                        x = (points[i].X + this.centr.X) / 2.0f;
                        y = (points[i].Y + this.centr.Y) / 2.0f;
                        points[i].SetXY(x, y);
                    }

                    // 8) Нахождение новой вершины смещением xi0   на половину расстояния к лучшей из вершин комплекса
                    // 9) Замена наихудшей точки
                    if (max)
                    {
                        while (points[i].FunctionValue <= pointFworst.FunctionValue)
                        {
                            x = (points[i].X + this.pointFbest.X) / 2.0f;
                            y = (points[i].Y + this.pointFbest.Y) / 2.0f;
                            points[i].SetXY(x, y);
                        }
                    }
                    else
                    {
                        while (points[i].FunctionValue >= pointFworst.FunctionValue)
                        {
                            x = (points[i].X + this.pointFbest.X) / 2.0f;
                            y = (points[i].Y + this.pointFbest.Y) / 2.0f;
                            points[i].SetXY(x, y);
                        }
                    }
                    break;
                }
            }
        }
       
        public PointOfFunction Solve()
        {
            List<PointOfFunction> points = GetComplex();
            bool end;
            do
            {
                GetBestAndWorstValue(points);
                if (pointFbest.FunctionValue == pointFworst.FunctionValue) { return points.Where(x => x.PointStatus == PointStatus.BEST).Select(el => el).Single(); }
                end = GetCoordCenter(points, this.ε);
                if (end) { return points.Where(x=>x.PointStatus == PointStatus.BEST).Select(el=>el).Single(); }
                GetPointInsteadWorstPoint(max, points, this.ε);
            }
            while (true);
        }
        public void RegisterMethod(VariableFunctionLimitations functionLimitations, functionTwoParam task)
        {
            this.pointFbest = new(task);
            this.pointFworst = new(task);
            this.centr = new(task);
            this.N = functionLimitations.countComplex;
            this.xmin = functionLimitations.Xmin;
            this.xmax = functionLimitations.Xmax;
            this.ymin = functionLimitations.Ymin;
            this.ymax = functionLimitations.Ymax;
            this.max = functionLimitations.TypeOfExtremum;
            this.k = functionLimitations.k;
            this.b = functionLimitations.b;
            this.sing = functionLimitations.Sing;
            this.ε = functionLimitations.ε;
            this.step = functionLimitations.step;
            this.task = task;
        }
        public ObservableCollection<Point3> GetChartData()
        {
            ObservableCollection<Point3> chart3dData = new();
            for (var i = this.xmin; i <= this.xmax; i += this.step) 
            {
                for (var j = this.ymin; j <= this.ymax; j += this.step)
                {
                    var point = new PointOfFunction(task);
                    point.SetXY(i, j);
                    chart3dData.Add(new Point3 { X = point.X, Y = point.Y, Z = point.FunctionValue });
                }
            }
            return chart3dData;
        }
        public List<List<Point3>> GetChartDataAsTable()
        {
            List<List<Point3>> data = new(); int row = 0;
            for (var i = this.xmin; i <= this.xmax; i += (this.step * 2), row++)
            {
                data.Add(new());
                for (var j = ymin; j <= this.ymax; j += (this.step * 2))
                {
                    var point = new PointOfFunction(task);
                    point.SetXY(i, j);
                    data[row].Add(new Point3 { X = point.X, Y = point.Y, Z = point.FunctionValue });
                }
            }
            return data;
        }
        public ObservableCollection<Point3> GetChartLimitationData()
        {
            ObservableCollection<Point3> limitationData = new();
            ObservableCollection<Point3> points = new();
            double yXmin = this.k * this.xmin + this.b;
            double yXmax = this.k * this.xmax + this.b;
            double xYmin = (this.ymin - this.b) / this.k;
            double xYmax = (this.ymax - this.b) / this.k;

            if (this.ymin <= yXmax && yXmax <= this.ymax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(this.xmax, yXmax);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            if (this.xmin <= xYmax && xYmax <= this.xmax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(xYmax, this.ymax);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            if (this.ymin <= yXmin && yXmin <= this.ymax) 
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(this.xmin, yXmin);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            if (this.xmin <= xYmin && xYmin <= this.xmax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(xYmin, this.ymin);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            return limitationData;
        }
    }

    // для функции 2-х переменных генетический алгоритм
    internal class GeneticAlgorithm:IMethod
    {
        #region поля
        public string Name { get; } = "Генетический алгоритм";
        private int N;                           // начальное число точек комплекса
        private PointOfFunction? pointFbest;     // лучшее значение функции
        private PointOfFunction? pointFworst;    // худшее значенеи функции
        private PointOfFunction? centr;          // центр комплекса
        private double xmin;                     // нижнее ограничение по х
        private double xmax;                     // верхнее ограничение по x
        private double ymin;                     // нижнее ограничение по y
        private double ymax;                     // верхнее ограничение по y
        private double k;                        // k: y=kx+b
        private double b;                        // b: y=kx+b
        private string sing;                     // знак ограничения второго рода
        private double ε;                        // точность
        private double step;
        private int round;
        private bool max;                        // тип экстремума
        private functionTwoParam task;           // задача
        #endregion
        private void GetBestAndWorstValue(List<PointOfFunction> points)
        {
            double maxV = Double.MinValue; int maxVid = 0;
            double minV = Double.MaxValue; int minVid = 0;
            foreach (var point in points) { point.PointStatus = PointStatus.NEUTRAL; }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].FunctionValue >= maxV) { maxV = points[i].FunctionValue; maxVid = i; }
                if (points[i].FunctionValue <= minV) { minV = points[i].FunctionValue; minVid = i; }
            }
            if (max)
            {
                if (maxVid == minVid) { maxVid = 0; minVid = 1; }
                points[maxVid].PointStatus = PointStatus.BEST;
                points[minVid].PointStatus = PointStatus.WORST;
                this.pointFbest.SetXY(points[maxVid].X, points[maxVid].Y);
                this.pointFbest.BelongingToComplex = points[maxVid].BelongingToComplex;
                this.pointFbest.PointStatus = points[maxVid].PointStatus;
                this.pointFworst.SetXY(points[minVid].X, points[minVid].Y);
                this.pointFworst.BelongingToComplex = points[minVid].BelongingToComplex;
                this.pointFworst.PointStatus = points[minVid].PointStatus;
            }
            else
            {
                if (maxVid == minVid) { maxVid = 0; minVid = 1; }
                points[maxVid].PointStatus = PointStatus.WORST;
                points[minVid].PointStatus = PointStatus.BEST;
                this.pointFbest.SetXY(points[minVid].X, points[minVid].Y);
                this.pointFbest.BelongingToComplex = points[minVid].BelongingToComplex;
                this.pointFbest.PointStatus = points[minVid].PointStatus;
                this.pointFworst.SetXY(points[maxVid].X, points[maxVid].Y);
                this.pointFworst.BelongingToComplex = points[maxVid].BelongingToComplex;
                this.pointFworst.PointStatus = points[maxVid].PointStatus;
            }
        }
        private bool CheckConditionSecondKind(double x, double y, string sing)
        {
            bool flag = false;
            switch (sing)
            {
                case "⩽": flag = (y <= this.k * x + this.b) ? true : false; break;
                case "⩾": flag = (y >= this.k * x + this.b) ? true : false; break;
            }
            return flag;
        }

        private List<PointOfFunction> GetStartPopulation(List<PointOfFunction> points)
        {
            double x = 0; double y = 0;
            Random rnd = new();
            double functionStepX = (this.xmax - this.xmin) / this.N;
            double functionStepY = (this.ymax - this.ymin) / this.N;
            for (double i = this.xmin; i <= this.xmax; i += functionStepX)
            {
                for (double j = this.ymin; j <= this.ymax; j += functionStepY)
                {
                    if (CheckConditionSecondKind(i, j, this.sing))
                    {
                        var point = new PointOfFunction(this.task);
                        point.SetXY(Math.Round(i, this.round), Math.Round(j, this.round));
                        points.Add(point);
                    }
                }
            }
            return points;
        }
        private List<PointOfFunction> DefinitionOfSurvivors(List<PointOfFunction> points)
        {
            Random rnd = new();
            int numberOfSurvivors;
            foreach (var point in points) { point.BelongingToComplex = false; }
            if(this.N < 500)
            {
                if (this.N < 20) { numberOfSurvivors = this.N; }
                else { numberOfSurvivors = (int)((rnd.Next(3, 10) * 10) * (this.N / 100.0f)); }
            }
            else
            {
                numberOfSurvivors = (int)((rnd.Next(2, 6) * 10) * (this.N / 100.0f));
            }
            if (max)
            {
                points = points.OrderByDescending(x => x.FunctionValue).Select(el => el).ToList();
            }
            else
            {
                points = points.OrderBy(x => x.FunctionValue).Select(el => el).ToList();
            }
            foreach(var point in points)
            {
                if (numberOfSurvivors > 0) { point.BelongingToComplex = true; numberOfSurvivors--; }
                else { break; }
            }
            return points;
        }
        private List<PointOfFunction> ReproductionandMutationDeath(List<PointOfFunction> points)
        {
            #region Рождение
            double x = -1;
            double y = -1;
            Random rnd = new();
            List<PointOfFunction> newborns = new();
            PointOfFunction[] firstPerentList = new PointOfFunction[this.N / 10];
            PointOfFunction[] secondPerentList = new PointOfFunction[this.N / 5];
            points.CopyTo(0, firstPerentList, 0, firstPerentList.Count());
            points.CopyTo(firstPerentList.Count(), secondPerentList, 0, secondPerentList.Count());
            int numberOfNewborns = (int)((rnd.Next(3, 7) * 10.0f) * (this.N / 100.0f));
            while(numberOfNewborns > 0)
            {
                int idFirstParent, idSecondParent;
                if (rnd.Next(-1, 1) < 0)
                {
                    idFirstParent = rnd.Next(0, firstPerentList.Count());
                    idSecondParent = rnd.Next(0, secondPerentList.Count());
                }
                else
                {
                    idFirstParent = rnd.Next(0, firstPerentList.Count());
                    idSecondParent = rnd.Next(0, firstPerentList.Count());
                }

                x = (firstPerentList[idFirstParent].X + secondPerentList[idSecondParent].X) / 2.0f;
                y = (firstPerentList[idFirstParent].Y + secondPerentList[idSecondParent].Y) / 2.0f;

                var point = new PointOfFunction(this.task);
                point.SetXY(Math.Round(x, this.round), Math.Round(y, this.round));
                point.BelongingToComplex = true;
                newborns.Add(point);
                numberOfNewborns--;
            }
            #endregion

            #region Мутация      
            int idMutation = -1;
            int numberOfMutation = (int)((rnd.Next(0, 5) * 10.0f) * (newborns.Count / 100.0f));
            while (numberOfMutation > 0)
            {
                idMutation = rnd.Next(0, newborns.Count);
                do
                {
                    x = newborns[idMutation].X + rnd.Next(-10, 11) * ((this.xmax - this.xmin) / 10.0f);
                    y = newborns[idMutation].Y + rnd.Next(-10, 11) * ((this.ymax - this.ymin) / 10.0f);

                    if (x < this.xmin) { x = xmin + ε; }
                    if (x > this.xmax) { x = xmax - ε; }
                    if (y < this.ymin) { y = ymin + ε; }
                    if (y > this.ymax) { y = ymax - ε; } 
                }
                while (!CheckConditionSecondKind(x, y, this.sing));
                newborns[idMutation].SetXY(Math.Round(x, this.round), Math.Round(y, this.round));
                newborns[idMutation].BelongingToComplex = CheckConditionSecondKind(x, y, this.sing);
                numberOfMutation--;
            }
            #endregion

            points.AddRange(newborns);

            points =  points.Where(x => x.BelongingToComplex == true).Select(el => el).ToList();

            this.N = points.Count;
            return points;
        }
        private bool Result(List<PointOfFunction> points)
        {
            double x = (points.Where(x => x.PointStatus != PointStatus.WORST).Sum(el => el.X)) / (this.N - 1);
            double y = (points.Where(x => x.PointStatus != PointStatus.WORST).Sum(el => el.Y)) / (this.N - 1);
            this.centr.SetXY(x, y);
            double cross1 = Math.Sqrt(Math.Pow(this.centr.X - this.pointFworst.X, 2)
                + Math.Pow(this.centr.Y - this.pointFworst.Y, 2));
            double cross2 = Math.Sqrt(Math.Pow(this.centr.X - this.pointFbest.X, 2)
                + Math.Pow(this.centr.Y - this.pointFbest.Y, 2));
            double cross3 = Math.Sqrt(Math.Pow(this.pointFbest.X - this.pointFworst.X, 2)
                + Math.Pow(this.pointFbest.Y - this.pointFworst.Y, 2));
            var p = (cross1 + cross2 + cross3) / 2;
            var S = Math.Sqrt(p * (p - cross1) * (p - cross2) * (p - cross3));
            bool flag = (Math.Round(S, this.round) < ε) ? true : false;
            return flag;
        }

        public PointOfFunction Solve()
        {
            List<PointOfFunction> points = new();
            points = GetStartPopulation(points);
            do
            {
                points = DefinitionOfSurvivors(points);
                points = ReproductionandMutationDeath(points);
                GetBestAndWorstValue(points);
            }
            while (!Result(points));
            return points.Where(x => x.PointStatus == PointStatus.BEST).Select(el => el).Single();
        }
        public void RegisterMethod(VariableFunctionLimitations functionLimitations, functionTwoParam task)
        {
            this.pointFbest = new(task);
            this.pointFworst = new(task);
            this.centr = new(task);
            this.N = functionLimitations.countComplex;
            this.xmin = functionLimitations.Xmin;
            this.xmax = functionLimitations.Xmax;
            this.ymin = functionLimitations.Ymin;
            this.ymax = functionLimitations.Ymax;
            this.max = functionLimitations.TypeOfExtremum;
            this.k = functionLimitations.k;
            this.b = functionLimitations.b;
            this.sing = functionLimitations.Sing;
            this.ε = functionLimitations.ε;
            this.step = functionLimitations.step;
            this.task = task;
            this.round = BitConverter.GetBytes(decimal.GetBits((decimal)this.ε)[3])[2];
        }
        public ObservableCollection<Point3> GetChartData()
        {
            ObservableCollection<Point3> chart3dData = new();
            for (var i = this.xmin; i <= this.xmax; i += this.step)
            {
                for (var j = this.ymin; j <= this.ymax; j += this.step)
                {
                    var point = new PointOfFunction(task);
                    point.SetXY(i, j);
                    chart3dData.Add(new Point3 { X = point.X, Y = point.Y, Z = point.FunctionValue });
                }
            }
            return chart3dData;
        }
        public List<List<Point3>> GetChartDataAsTable()
        {
            List<List<Point3>> data = new(); int row = 0;
            for (var i = this.xmin; i <= this.xmax; i += (this.step * 2), row++)
            {
                data.Add(new());
                for (var j = ymin; j <= this.ymax; j += (this.step * 2))
                {
                    var point = new PointOfFunction(task);
                    point.SetXY(i, j);
                    data[row].Add(new Point3 { X = point.X, Y = point.Y, Z = point.FunctionValue });
                }
            }
            return data;
        }
        public ObservableCollection<Point3> GetChartLimitationData()
        {
            ObservableCollection<Point3> limitationData = new();
            ObservableCollection<Point3> points = new();
            double yXmin = this.k * this.xmin + this.b;
            double yXmax = this.k * this.xmax + this.b;
            double xYmin = (this.ymin - this.b) / this.k;
            double xYmax = (this.ymax - this.b) / this.k;

            if (this.ymin <= yXmax && yXmax <= this.ymax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(this.xmax, yXmax);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            if (this.xmin <= xYmax && xYmax <= this.xmax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(xYmax, this.ymax);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            if (this.ymin <= yXmin && yXmin <= this.ymax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(this.xmin, yXmin);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            if (this.xmin <= xYmin && xYmin <= this.xmax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(xYmin, this.ymin);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            return limitationData;
        }
    }

    // для функции 2-х переменных метод Пчелинный улей
    internal class BeeColonyMethod: IMethod
    {
        #region поля
        public string Name { get; } = "Пчелиная колония";
        private PointOfFunction? centr;          // центр комплекса
        private int N;                           // начальное число точек комплекса
        private double radiusOfSearchAreaX;      // радиус области поиска Х
        private double radiusOfSearchAreaY;      // радиус области поиска Y
        private int numberOfBestArea;            // кол-во лучших точек
        private int numberOfNormArea;            // кол-во нормальных точек
        private int numberOfScoutBees;           // кол-во пчел разведчиков
        private int numberOfBeesInNormalArea;    // кол-во пчел на нормальных точках
        private int numberOfBeesInBestArea;      // кол-во пчел на лучших точках
        private double bestFunctionValue;
        private double xmin;                     // нижнее ограничение по х
        private double xmax;                     // верхнее ограничение по x
        private double ymin;                     // нижнее ограничение по y
        private double ymax;                     // верхнее ограничение по y
        private double k;                        // k: y=kx+b
        private double b;                        // b: y=kx+b
        private string sing;                     // знак ограничения второго рода
        private double ε;                        // точность
        private double step;                     // шаг построения графика
        private bool max;                        // тип экстремума
        private functionTwoParam task;           // задача
        private int round;
        #endregion

        private bool CheckConditionSecondKind(double x, double y, string sing)
        {
            bool flag = false;
            switch (sing)
            {
                case "⩽": flag = (y <= k * x + b) ? true : false; break;
                case "⩾": flag = (y >= k * x + b) ? true : false; break;
            }
            return flag;
        }

        private List<PointOfFunction> ExplorationFirst()
        {
            double x = -1;
            double y = -1;
            bool flag = true;
            Random rnd = new();
            List<PointOfFunction> points = new();

            while (points.Count() < this.numberOfScoutBees)
            {
                do
                {
                    x = this.xmin + (rnd.Next(1, 101) / 100.0f) * (this.xmax - this.xmin);
                    y = this.ymin + (rnd.Next(1, 101) / 100.0f) * (this.ymax - this.ymin);
                }
                while (!CheckConditionSecondKind(x, y, this.sing));

                var point = new PointOfFunction(this.task);
                point.SetXY(x, y);
                point.BelongingToComplex = true;

                foreach (var el in points)
                {
                    flag = true;
                    if ((el.X - this.radiusOfSearchAreaX) < point.X && point.X < (el.X + this.radiusOfSearchAreaX) &&
                        (el.Y - this.radiusOfSearchAreaY) < point.Y && point.Y < (el.Y + this.radiusOfSearchAreaY))
                    {
                        flag = false;
                        if (max && el.FunctionValue < point.FunctionValue)
                        {
                            el.SetXY(point.X, point.Y);
                            el.BelongingToComplex = true;
                        }
                        if (!max && el.FunctionValue > point.FunctionValue)
                        {
                            el.SetXY(point.X, point.Y);
                            el.BelongingToComplex = true;
                        }
                        break;
                    }
                }
                if (flag) { points.Add(point); }
            }
            if (max)
            {
                return points.OrderByDescending(x => x.FunctionValue).Select(el => el).ToList();
            }
            else
            {
                return points.OrderBy(x => x.FunctionValue).Select(el => el).ToList();
            }
        }
        private List<PointOfFunction> ExplorationSecond(List<PointOfFunction> centrArea)
        {
            double x = -1;
            double y = -1;
            Random rnd = new();
            List<PointOfFunction> areaPoints = new();

            for (int id = 0; id < this.numberOfBestArea; id++)
            {
                for (int i = 0; i < this.numberOfBeesInBestArea; i++)
                {
                    do
                    {
                        x = (centrArea[id].X - this.radiusOfSearchAreaX) + (rnd.Next(1, 101) / 100.0f) * (this.radiusOfSearchAreaX * 2.0f);
                        y = (centrArea[id].Y - this.radiusOfSearchAreaY) + (rnd.Next(1, 101) / 100.0f) * (this.radiusOfSearchAreaY * 2.0f);

                        if (x < this.xmin) { x = this.xmin + ε; }
                        if (x > this.xmax) { y = this.xmax - ε; }
                        if (y < this.ymin) { y = this.ymin + ε; }
                        if (y > this.ymax) { y = this.ymax - ε; }
                    }
                    while (!CheckConditionSecondKind(x, y, this.sing));
                    var point = new PointOfFunction(this.task);
                    point.SetXY(x, y);
                    point.BelongingToComplex = true;
                    areaPoints.Add(point);
                }
            }
            for (int id = 0; id < this.numberOfNormArea; id++)
            {
                int index = id + this.numberOfBestArea;
                for (int i = 0; i < this.numberOfBeesInNormalArea; i++)
                {
                    do
                    {
                        x = (centrArea[index].X - this.radiusOfSearchAreaX) + (rnd.Next(1, 101) / 100.0f) * (this.radiusOfSearchAreaX * 2.0f);
                        y = (centrArea[index].Y - this.radiusOfSearchAreaY) + (rnd.Next(1, 101) / 100.0f) * (this.radiusOfSearchAreaY * 2.0f);

                        if (x < this.xmin) { x = this.xmin + ε; }
                        if (x > this.xmax) { y = this.xmax - ε; }
                        if (y < this.ymin) { y = this.ymin + ε; }
                        if (y > this.ymax) { y = this.ymax - ε; }
                    }
                    while (!CheckConditionSecondKind(x, y, this.sing));
                    var point = new PointOfFunction(this.task);
                    point.SetXY(x, y);
                    point.BelongingToComplex = true;
                    areaPoints.Add(point);
                }
            }
            if (max)
            {
                return
                    areaPoints
                    .OrderByDescending(x => x.FunctionValue)
                    .Select(el => el)
                    .ToList();
            }
            else
            {
                return
                    areaPoints
                    .OrderBy(x => x.FunctionValue)
                    .Select(el => el)
                    .ToList();
            }
        }
        private List<PointOfFunction> ExplorationThird(List<PointOfFunction> currentCenterPoints)
        {
            double x = -1;
            double y = -1;
            bool flag = true;
            Random rnd = new();
            List<PointOfFunction> points = new();
            while (points.Count() < this.numberOfScoutBees)
            {
                do
                {
                    x = this.xmin + (rnd.Next(1, 101) / 100.0f) * (this.xmax - this.xmin);
                    y = this.ymin + (rnd.Next(1, 101) / 100.0f) * (this.ymax - this.ymin);
                }
                while (!CheckConditionSecondKind(x, y, this.sing));

                var point = new PointOfFunction(this.task);
                point.SetXY(x, y);
                point.BelongingToComplex = true;

                foreach (var el in currentCenterPoints)
                {
                    flag = true;
                    if (el.X == point.X && el.Y == point.Y) { flag = false; break; }
                }
                if (flag)
                {
                    foreach (var el in points)
                    {
                        flag = true;
                        if ((el.X - this.radiusOfSearchAreaX) < point.X && point.X < (el.X + this.radiusOfSearchAreaX) &&
                            (el.Y - this.radiusOfSearchAreaY) < point.Y && point.Y < (el.Y + this.radiusOfSearchAreaY))
                        {
                            flag = false;
                            if (max && el.FunctionValue < point.FunctionValue)
                            {
                                el.SetXY(point.X, point.Y);
                                el.BelongingToComplex = true;
                            }
                            if (!max && el.FunctionValue > point.FunctionValue)
                            {
                                el.SetXY(point.X, point.Y);
                                el.BelongingToComplex = true;
                            }
                            break;
                        }
                    }
                }
                if (flag) { points.Add(point); }
            }
            points.AddRange(currentCenterPoints);
            if (max)
            {
                return 
                    points
                    .OrderByDescending(x => x.FunctionValue)
                    .Select(el => el)
                    .ToList()
                    .GetRange(0, this.numberOfScoutBees);
            }
            else
            {
                return 
                    points
                    .OrderBy(x => x.FunctionValue)
                    .Select(el => el)
                    .ToList()
                    .GetRange(0, this.numberOfScoutBees);
            }
        }

        private bool Result(List<PointOfFunction> points)
        {
            if (this.max && points[0].FunctionValue > this.bestFunctionValue) 
            {
                this.radiusOfSearchAreaX -= radiusOfSearchAreaX * 0.01f;
                this.radiusOfSearchAreaY -= radiusOfSearchAreaY * 0.01f;
            }
            if (!this.max && points[0].FunctionValue < this.bestFunctionValue)
            {
                this.radiusOfSearchAreaX -= radiusOfSearchAreaX * 0.01f;
                this.radiusOfSearchAreaY -= radiusOfSearchAreaY * 0.01f;
            }
            if (Math.Abs(this.bestFunctionValue - points[0].FunctionValue) < this.ε)
            {
                double x = points.Sum(el => el.X) / points.Count();
                double y = points.Sum(el => el.Y) / points.Count();
                this.centr.SetXY(x, y);
                double cross1 = Math.Sqrt(Math.Pow(this.centr.X - points[points.Count - 1].X, 2)
                    + Math.Pow(this.centr.Y - points[points.Count - 1].Y, 2));
                double cross2 = Math.Sqrt(Math.Pow(this.centr.X - points[0].X, 2)
                    + Math.Pow(this.centr.Y - points[0].Y, 2));
                double cross3 = Math.Sqrt(Math.Pow(points[0].X - points[points.Count - 1].X, 2)
                    + Math.Pow(points[0].Y - points[points.Count - 1].Y, 2));
                var p = (cross1 + cross2 + cross3) / 2;
                var S = Math.Sqrt(p * (p - cross1) * (p - cross2) * (p - cross3));
                return S < this.ε ? true : false;
            }
            this.bestFunctionValue = points[0].FunctionValue;
            return false;
        }

        public PointOfFunction Solve()
        {
            List<PointOfFunction> points = ExplorationFirst();
            do
            {
                points = ExplorationSecond(points);
                points = ExplorationThird(points);
            }
            while (!Result(points));
            return points[0];
        }
        public void RegisterMethod(VariableFunctionLimitations functionLimitations, functionTwoParam task)
        {
            this.centr = new(task);
            this.xmin = functionLimitations.Xmin;
            this.xmax = functionLimitations.Xmax;
            this.ymin = functionLimitations.Ymin;
            this.ymax = functionLimitations.Ymax;
            this.max = functionLimitations.TypeOfExtremum;
            this.k = functionLimitations.k;
            this.b = functionLimitations.b;
            this.sing = functionLimitations.Sing;
            this.ε = functionLimitations.ε;
            this.step = functionLimitations.step;
            this.task = task;
            if (max) { this.bestFunctionValue = Double.MinValue; }
            else { this.bestFunctionValue = Double.MaxValue; }
            this.N = functionLimitations.countComplex;
            this.round = BitConverter.GetBytes(decimal.GetBits((decimal)this.ε)[3])[2];
            this.radiusOfSearchAreaX = Math.Round((this.xmax - this.xmin) * 0.05f, this.round);
            this.radiusOfSearchAreaY = Math.Round((this.ymax - this.ymin) * 0.05f, this.round);
            this.numberOfBestArea = (int)(this.N * 0.2f / 3);
            this.numberOfNormArea = numberOfBestArea * 2;
            this.numberOfScoutBees = numberOfBestArea + numberOfNormArea;
            this.numberOfBeesInNormalArea = (this.N - numberOfScoutBees) / (2 * numberOfNormArea);
            this.numberOfBeesInBestArea = (this.N - numberOfScoutBees) / (2 * numberOfBestArea);
        }
        public ObservableCollection<Point3> GetChartData()
        {
            ObservableCollection<Point3> chart3dData = new();
            for (var i = this.xmin; i <= this.xmax; i += this.step)
            {
                for (var j = this.ymin; j <= this.ymax; j += this.step)
                {
                    var point = new PointOfFunction(task);
                    point.SetXY(i, j);
                    chart3dData.Add(new Point3 { X = point.X, Y = point.Y, Z = point.FunctionValue });
                }
            }
            return chart3dData;
        }
        public List<List<Point3>> GetChartDataAsTable()
        {
            List<List<Point3>> data = new(); int row = 0;
            for (var i = this.xmin; i <= this.xmax; i += (this.step * 2), row++)
            {
                data.Add(new());
                for (var j = ymin; j <= this.ymax; j += (this.step * 2))
                {
                    var point = new PointOfFunction(task);
                    point.SetXY(i, j);
                    data[row].Add(new Point3 { X = point.X, Y = point.Y, Z = point.FunctionValue });
                }
            }
            return data;
        }
        public ObservableCollection<Point3> GetChartLimitationData()
        {
            ObservableCollection<Point3> limitationData = new();
            ObservableCollection<Point3> points = new();
            double yXmin = this.k * this.xmin + this.b;
            double yXmax = this.k * this.xmax + this.b;
            double xYmin = (this.ymin - this.b) / this.k;
            double xYmax = (this.ymax - this.b) / this.k;

            if (this.ymin <= yXmax && yXmax <= this.ymax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(this.xmax, yXmax);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            if (this.xmin <= xYmax && xYmax <= this.xmax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(xYmax, this.ymax);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            if (this.ymin <= yXmin && yXmin <= this.ymax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(this.xmin, yXmin);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            if (this.xmin <= xYmin && xYmin <= this.xmax)
            {
                var point = new PointOfFunction(this.task);
                point.SetXY(xYmin, this.ymin);
                limitationData.Add(new Point3 { X = point.X, Y = point.Y });
            }
            return limitationData;
        }
    }

    internal static class Methodlist
    {
        public static List<IMethod> methods = new()
        {
            new ComplexBoxingMethod(),
            new GeneticAlgorithm(),
            new BeeColonyMethod()
        };
    }
}
