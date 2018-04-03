using System;
using MarchingCubes.CommonTypes;

namespace MarchingCubes.Algoritms
{
    /// <summary>
    /// Чисельне знаходження значення похідної в точці
    /// </summary>
    public class Derivative
    {
        private DerivationAccuracy accurracy;
        private FunctionHolder function;
        private double h;

        public Derivative(FunctionHolder function)
            : this(function, DerivationAccuracy.Normal)
        {
        }

        public Derivative(FunctionHolder function, DerivationAccuracy accurracy)
        {
            this.function = function.CloneFunctionHolder();
            this.accurracy = accurracy;
            h = 0.00001;
        }

        public DerivationAccuracy Accurracy
        {
            get { return accurracy; }
            set { accurracy = value; }
        }

        public double H
        {
            get { return h; }
            set { h = value; }
        }

        public Arguments GetAntiGradient(Arguments point)
        {
            Arguments item = GetGradient(point);
            item.Revert();
            return item;
        }

        public Arguments GetGradient(Arguments point)
        {
            var gradient = new Arguments();
            int index = 1;
            foreach (Variable item in point)
            {
                gradient.Add(new Variable(GetDerivate(point, index)));
                index++;
            }
            return gradient;
        }

        public double GetDerivate(Arguments inArgs, int special)
        {
            if (function == null)
                throw new ArgumentException("function is not set to object");

            if (special > inArgs.Count)
                throw new ArgumentException(String.Format("Function cannot find argument number {0}", special));

            if (inArgs.Count != function.Dimension)
                throw new ArgumentException(String.Format("Arguments count does not match function dimension"));

            Arguments args = inArgs.CloneArguments();

            if (special != -1)
            {
                int index = 1;
                foreach (Variable item in args)
                {
                    if (index != special)
                    {
                        item.IsConstant = true;
                        item.Value = 0;
                    }
                    index++;
                }
            }
            if (accurracy == DerivationAccuracy.Normal)
            {
                return (function.Calculate(args + h) - function.Calculate(args - h)) / (2 * h);
            }
            else if (accurracy == DerivationAccuracy.High)
            {
                return (-function.Calculate(args + h * 2) + 8 * function.Calculate(args + h) -
                        8 * function.Calculate(args - h) + function.Calculate(args - h * 2)) / (12 * h);
            }
            else
            {
                return 0;
            }
        }

        public double GetDerivate(Arguments args)
        {
            return GetDerivate(args, -1);
        }
    }

    public enum DerivationAccuracy : byte
    {
        Normal,
        High
    }
}