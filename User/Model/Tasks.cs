using ModelsView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    internal interface ITask
    {
        public string? Name { get; }
        public double GetTask(Point2 point);
        public void RegisterTask(List<TaskParameterValueView> parameter);
    }
    internal class RegisterTask15: ITask
    {
        public string? Name { get; } = "Вариант 15";
        private double a;
        private double β;
        private double y;
        private double p1;
        private double p2;
        private double N;
        public void RegisterTask(List<TaskParameterValueView> parameter)
        {
            this.a = parameter.Where(x=>x.Notation == "α").Select(el=>el.Value).Single();
            this.β = parameter.Where(x => x.Notation == "β").Select(el => el.Value).Single();
            this.y = parameter.Where(x => x.Notation == "γ").Select(el => el.Value).Single();
            this.p1 = parameter.Where(x => x.Notation == "∆р1").Select(el => el.Value).Single();
            this.p2 = parameter.Where(x => x.Notation == "∆р2").Select(el => el.Value).Single();
            this.N = parameter.Where(x => x.Notation == "N").Select(el => el.Value).Single();
        }
        public double GetTask(Point2 point)
        {
            double sqrt = Math.Sqrt(Math.Pow(point.X, N) + Math.Pow(point.Y, N));
            double FunctionValue = a * (point.X - β * p1) * Math.Cos(y * p2 * sqrt);
            return FunctionValue;
        }
    }
    internal class RegisterTask18: ITask
    {
        public string? Name { get; } = "Вариант 18";
        private double a;
        private double β;
        private double μ;
        private double A;
        private double G;
        private double N;
        public void RegisterTask(List<TaskParameterValueView> parameter)
        {
            this.a = parameter.Where(x => x.Notation == "α").Select(el => el.Value).Single();
            this.β = parameter.Where(x => x.Notation == "β").Select(el => el.Value).Single();
            this.μ = parameter.Where(x => x.Notation == "γ").Select(el => el.Value).Single();
            this.A = parameter.Where(x => x.Notation == "A").Select(el => el.Value).Single();
            this.G = parameter.Where(x => x.Notation == "G").Select(el => el.Value).Single();
            this.N = parameter.Where(x => x.Notation == "N").Select(el => el.Value).Single();
        }
        public double GetTask(Point2 point)
        {
            double FunctionValue = a * (G * μ * (Math.Pow(point.Y - point.X, N) + Math.Pow(β * A - point.X, N)));
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
