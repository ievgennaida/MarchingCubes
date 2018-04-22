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
    public class GoldenSectionSearch : INterpolationAlgoritm
    {
        protected MarchingFunction function;
        protected double accuracyEpsilon;
        public GoldenSectionSearch(MarchingFunction function, double accuracyEpsilon = 0.05)
        {
            this.function = function;
            this.accuracyEpsilon = accuracyEpsilon;
        }

        /// <summary>
        /// Find the nearest point.
        /// </summary>
        public Arguments Interpolate(Arguments start, Arguments finish, int axissIndex, double? isolevel=null)
        {
            var startValue = start[axissIndex].Value > finish[axissIndex].Value ? finish[axissIndex].Value : start[axissIndex].Value;
            var finishValue = start[axissIndex].Value > finish[axissIndex].Value ? start[axissIndex].Value : finish[axissIndex].Value;
            var curA = startValue;
            var curB = finishValue;
            double funcA = 0;
            double funcB = 0;
            double calcA = 0;
            double calcB = 0;

            var point = start.CloneArguments();
            // Some constants
            var c = (3 - Math.Sqrt(5)) / 2;
            var fi = (Math.Sqrt(5) + 1) / 2; 
            var calculated = Calculated.FirstIteration;
            for (; ; )
            {
                if ((curB - curA < accuracyEpsilon) && calculated != Calculated.FirstIteration)
                {
                    var pointToReturn = point.CloneArguments();
                    if (GetNearestOrMin(funcA, funcB, isolevel))
                    {
                        pointToReturn[axissIndex] = curA;
                    }
                    else
                    {
                        pointToReturn[axissIndex] = curB;
                    }

                    return pointToReturn;
                }

                calcA = curA + c * (curB - curA);
                point[axissIndex] = calcA;
                funcA = function.Calculate(point);

                calcB = curA + (1 - c) * (curB - curA);
                point[axissIndex] = calcB;
                funcB = function.Calculate(point);

                var result = GetNearestOrMin(funcA, funcB, isolevel);

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

        private static bool GetNearestOrMin(double funcA, double funcB, double? minium=null)
        {
            bool result;
            if (minium.HasValue)
            {
                result = GetLenght(minium.Value, funcA) < GetLenght(minium.Value, funcB);
            }
            else
            {
                result = funcA <= funcB;
            }

            return result;
        }


        protected enum Calculated
        {
            FirstIteration,
            A,
            B
        }
    }
}