using ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;

namespace User.Model
{
    internal interface ITask
    {
        public string[] Name { get; }
        public double GetTask(double x, double y);
        public void RegisterTask(List<TaskParameterValueView> parameter);
    }
    internal class RegisterTask15: ITask
    {
        public string[] Name { get; } = { "Вариант 15" };
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
    }
    internal class RegisterTask18: ITask
    {
        public string[] Name { get; } = { "Вариант 18" };
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
    }
    internal class RegisterTask19 : ITask
    {
        public string[] Name { get; } = { "Вариант 19" };
        private double Tr;
        private double b;
        private double W;
        private double p;
        private double c;
        private double T0;
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
            
            double FunctionValue = this.a * this.G * this.μ * pow * this.Cs;
            return FunctionValue;
        }
    }
    internal static class Tasklist
    {
        public static List<ITask> tasks = new List<ITask> 
        { 
            new RegisterTask15(), 
            new RegisterTask18() 
        };
    }
}
