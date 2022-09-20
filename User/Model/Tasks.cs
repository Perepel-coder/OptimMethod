using ModelsView;
using OptimizationMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace User.Model
{
    internal interface ITask
    {
        public string Name { get; }
        public double GetTask(double x, double y);
        public void RegisterTask(List<TaskParameterValueView> parameter);
        public string OutputResult(PointOfFunction result);
    }
    internal class RegisterTask15: ITask
    {
        public string Name { get; } = "Вариант 15";
        private double a;
        private double β;
        private double y;
        private double p1;
        private double p2;
        private double N;
        private double t;
        public void RegisterTask(List<TaskParameterValueView> parameter)
        {
            this.a = parameter.Where(x=>x.Notation == "α").Select(el=>el.Value).Single();
            this.β = parameter.Where(x => x.Notation == "β").Select(el => el.Value).Single();
            this.y = parameter.Where(x => x.Notation == "γ").Select(el => el.Value).Single();
            this.p1 = parameter.Where(x => x.Notation == "∆р1").Select(el => el.Value).Single();
            this.p2 = parameter.Where(x => x.Notation == "∆р2").Select(el => el.Value).Single();
            this.N = parameter.Where(x => x.Notation == "N").Select(el => el.Value).Single();
            this.t = parameter.Where(x => x.Notation == "t").Select(el => el.Value).Single();
        }
        public double GetTask(double x, double y)
        {
            double sqrt = Math.Sqrt(Math.Pow(x, this.N) + Math.Pow(y, this.N));
            double FunctionValue = this.a * (x - this.β * this.p1) * Math.Cos(this.y * this.p2 * sqrt) * this.t;
            return FunctionValue;
        }
        public string OutputResult(PointOfFunction result)
        {
            return 
                $"Значение целевой функции в точке\n" +
                $"X = {result.X}\n" +
                $"Y = {result.Y}\n" +
                $"Z(X, Y) = {result.FunctionValue}";
        }
    }
    internal class RegisterTask18: ITask
    {
        public string Name { get; } = "Вариант 18";
        private double a;
        private double β;
        private double μ;
        private double A;
        private double G;
        private double N;
        private double Cs;
        public void RegisterTask(List<TaskParameterValueView> parameter)
        {
            this.a = parameter.Where(x => x.Notation == "α").Select(el => el.Value).Single();
            this.β = parameter.Where(x => x.Notation == "β").Select(el => el.Value).Single();
            this.μ = parameter.Where(x => x.Notation == "γ").Select(el => el.Value).Single();
            this.A = parameter.Where(x => x.Notation == "A").Select(el => el.Value).Single();
            this.G = parameter.Where(x => x.Notation == "G").Select(el => el.Value).Single();
            this.N = parameter.Where(x => x.Notation == "N").Select(el => el.Value).Single();
            this.Cs = parameter.Where(x => x.Notation == "Cs").Select(el => el.Value).Single();
        }
        public double GetTask(double x, double y)
        {
            double pow = Math.Pow(y - x, this.N) + Math.Pow(this.β * this.A - x, this.N);
            double FunctionValue = this.a * this.G * this.μ * pow * this.Cs;
            return FunctionValue;
        }
        public string OutputResult(PointOfFunction result)
        {
            return
                $"Значение целевой функции в точке\n" +
                $"X = {result.X}\n" +
                $"Y = {result.Y}\n" +
                $"Z(X, Y) = {result.FunctionValue}";
        }
    }
    internal class RegisterTask19 : ITask
    {
        public string Name { get; } = "Вариант 19";
        private double Tr;
        private double H;
        private double W;
        private double L;
        private double F;
        private double au;
        private double μ0;
        private double n;
        private double b;
        private double p;
        private double c;
        private double T0;
        public void RegisterTask(List<TaskParameterValueView> parameter)
        {
            this.Tr = parameter.Where(x => x.Notation == "Tr").Select(el => el.Value).Single();
            this.H = parameter.Where(x => x.Notation == "H").Select(el => el.Value).Single();
            this.W = parameter.Where(x => x.Notation == "W").Select(el => el.Value).Single();
            this.L = parameter.Where(x => x.Notation == "L").Select(el => el.Value).Single();
            this.au = parameter.Where(x => x.Notation == "au").Select(el => el.Value).Single();
            this.μ0 = parameter.Where(x => x.Notation == "μ0").Select(el => el.Value).Single();
            this.n = parameter.Where(x => x.Notation == "n").Select(el => el.Value).Single();
            this.b = parameter.Where(x => x.Notation == "b").Select(el => el.Value).Single();
            this.p = parameter.Where(x => x.Notation == "p").Select(el => el.Value).Single();
            this.c = parameter.Where(x => x.Notation == "c").Select(el => el.Value).Single();
            this.T0 = parameter.Where(x => x.Notation == "T0").Select(el => el.Value).Single();

            var h_w = (double)this.H / (double)this.W;
            this.F = (0.125f * h_w * h_w) - (0.625f * h_w) + 1.0f;
        }
        public double GetTask(double x, double y)
        {   
            double Qch = ((this.H * this.W * x) / 2.0f) * this.F;
            double ym = (double)x / (double)this.H;
            var t = Math.Pow(ym, this.n + 1);
            double qy = this.H * this.W * this.μ0 * Math.Pow(ym, this.n + 1);
            double qa = this.W * this.au * (Math.Pow(this.b, -1) - y + this.Tr);

            double calculation1 = (this.b * qy + this.W * this.au) / (this.b * qa);
            double calculation2 = 1 - Math.Exp((-this.b * qa * this.L) / (this.p * this.c * Qch));
            double calculation3 = Math.Exp(this.b * (this.T0 - this.Tr - ((qa * this.L) / (this.p * this.c * Qch))));

            double T = this.Tr + (1.0f/this.b) * Math.Log(calculation1 * calculation2 + calculation3);
            //double η = this.μ0 * Math.Exp(-this.b * (T - this.Tr)) * Math.Pow(ym, this.n - 1);
            return T;
        }
        public string OutputResult(PointOfFunction result)
        {
            double ym = result.X / this.H;
            double η = this.μ0 * Math.Exp(-this.b * (result.FunctionValue - this.Tr)) * Math.Pow(ym, this.n - 1);
            double Qch = ((this.H * this.W * result.X) / 2.0f) * this.F;
            double Q = this.p * Qch * 3600.0f;
            return
                $"Скорость движения крышки канала X (Vu) = {result.X} м/с\n" +
                $"Tемпература крышки канала Y (Tu) = {result.Y} °С\n" +
                $"Температура смеси Z (T) = {result.FunctionValue} °С\n" +
                $"Вязкость материала в канале Z (η) = {Math.Round(η, 4)} Па*с\n" +
                $"Производительность канала (Q) = {Math.Round(Q, 4)} кг/ч\n";
        }
    }
    internal static class Tasklist
    {
        public static List<ITask> tasks = new List<ITask> 
        { 
            new RegisterTask15(), 
            new RegisterTask18(),
            new RegisterTask19()
        };
    }
}
