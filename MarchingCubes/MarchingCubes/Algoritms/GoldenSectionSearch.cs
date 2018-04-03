using System;
using MarchingCubes.CommonTypes;
using MarchingCubes.GraphicTypes;
using MarchingCubes.Algoritms;

namespace MarchingCubes.Algoritms
{
    /// <summary>
    /// The golden-section search is a technique for finding the extremum (minimum or maximum) 
    /// of a strictly unimodal function by successively narrowing the range of values inside which the extremum is known to exist. 
    /// </summary>
    public class GoldenSectionSearch : AlgoritmBase, IOptimizationAlgoritm
    {
        private double c;
        private double fi;
        private double finish;
        private double start;

        public GoldenSectionSearch(FunctionHolder function)
            : base(function, null)
        {
        }

        public GoldenSectionSearch(FunctionHolder function, double start, double finish, double accuracyEpsilon)
            : base(function, null, accuracyEpsilon)
        {
            this.start = start;
            this.finish = finish;
        }

        public GoldenSectionSearch(FunctionHolder function, double start, double finish)
            : base(function, null)
        {
            this.start = start;
            this.finish = finish;
        }

        public double Start
        {
            get { return start; }
            set { start = value; }
        }

        public double Finish
        {
            get { return finish; }
            set { finish = value; }
        }

        private double C
        {
            get
            {
                if (c == 0)
                    c = (3 - Math.Sqrt(5)) / 2;
                return c;
            }
        }

        private double Fi
        {
            get
            {
                if (fi == 0)
                    fi = (Math.Sqrt(5) + 1) / 2;
                return fi;
            }
        }

        public Arguments GetMinium()
        {
            return GetMinium(start, finish);
        }

        public Arguments GetMinium(double start, double finish)
        {
            return GetMinium(new Arguments(start), new Arguments(finish), new Arguments(0), 0, false, 0);
        }


        /// <summary>
        /// Find the nearest point.
        /// </summary>
        public Arguments GetMinium(Arguments start, Arguments finish, Arguments point, int index, bool useMinium,
                                   double minium)
        {
            this.start = start[index].Value > finish[index].Value ? finish[index].Value : start[index].Value;
            this.finish = start[index].Value > finish[index].Value ? start[index].Value : finish[index].Value;
            double curA = this.start;
            double curB = this.finish;
            double funcA = 0;
            double funcB = 0;
            double calcA = 0;
            double calcB = 0;

            Calculated calculated = Calculated.FirstIteration;
            for (; ; )
            {
                if ((curB - curA < AccuracyEpsilon) && calculated != Calculated.FirstIteration)
                {
                    Arguments pointToReturn = point.CloneArguments();
                    if (IsValueInFuncABetter(useMinium, minium, funcA, funcB))
                    {
                        pointToReturn[index] = curA;
                    }
                    else
                    {
                        pointToReturn[index] = curB;
                    }
                    return pointToReturn;
                }

                calcA = curA + C * (curB - curA);
                point[index] = calcA;
                funcA = function.Calculate(point);

                calcB = curA + (1 - C) * (curB - curA);
                point[index] = calcB;
                funcB = function.Calculate(point);

                bool result = IsValueInFuncABetter(useMinium, minium, funcA, funcB);

                if (result)
                {
                    curB = calcB;
                    calculated = Calculated.B;
                }
                else
                {
                    curA = calcA;
                    calculated = Calculated.A;
                }
            }
        }

        public static double GetLenght(double one, double two)
        {
            return Math.Sqrt(Math.Pow((two - one), 2));
        }

        private static bool IsValueInFuncABetter(bool useMinium, double minium, double funcA, double funcB)
        {
            bool result;
            if (useMinium)
            {
                result = GetLenght(minium, funcA) < GetLenght(minium, funcB);
            }
            else
            {
                result = funcA <= funcB;
            }
            return result;
        }

        public double GetMiniumValue()
        {
            return function.Calculate(GetMinium());
        }

        #region Nested type: Calculated

        private enum Calculated
        {
            FirstIteration,
            A,
            B
        }

        #endregion
    }
}