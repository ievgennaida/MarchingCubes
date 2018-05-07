using MarchingCubes.CommonTypes;
using System;

namespace MarchingCubes.Algoritms
{
    /// <summary>
    /// The golden-section search is a technique for finding the extremum (minimum or maximum) 
    /// of a strictly unimodal function by successively narrowing the range of values inside which the extremum is known to exist. 
    /// </summary>
    public class GoldenSectionSearch : IinterpolationAlgoritm
    {
        protected MarchingFunction function;
        protected double accuracyEpsilon;
        public GoldenSectionSearch(MarchingFunction function, double accuracyEpsilon = 0.01)
        {
            this.function = function;
            this.accuracyEpsilon = accuracyEpsilon;
        }

        private double? Calc(Point point)
        {
            if(function==null)
            {
                return null;
            }

            try
            {
               var toReturn =  function(point);
                return toReturn;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Find the nearest point.
        /// </summary>
        public Point Interpolate(Point start, Point finish, int axissIndex, double? isolevel = null)
        {
            var startValue = start.Get(axissIndex) > finish.Get(axissIndex) ? finish.Get(axissIndex) : start.Get(axissIndex);
            var finishValue = start.Get(axissIndex) > finish.Get(axissIndex) ? start.Get(axissIndex) : finish.Get(axissIndex);
            var curA = startValue;
            var curB = finishValue;
            double funcA = 0;
            double funcB = 0;
            double calcA = 0;
            double calcB = 0;

            var point = start.Clone();
            // Some constants
            var c = (3 - Math.Sqrt(5)) / 2;
            var fi = (Math.Sqrt(5) + 1) / 2;
            var calculated = Calculated.FirstIteration;
            for (; ; )
            {
                if ((curB - curA < accuracyEpsilon) && calculated != Calculated.FirstIteration)
                {
                    var pointToReturn = point.Clone();
                    if (GetNearestOrMin(funcA, funcB, isolevel))
                    {
                        pointToReturn.Set(axissIndex, curA);
                    }
                    else
                    {
                        pointToReturn.Set(axissIndex, curB);
                    }

                    return pointToReturn;
                }

                calcA = curA + c * (curB - curA);
                point.Set(axissIndex, calcA);

                var toSet = Calc(point);
                calcB = curA + (1 - c) * (curB - curA);
                point.Set(axissIndex, calcB);

                var toSet2 = Calc(point);

                if (toSet.HasValue)
                {
                    funcA = toSet.Value;
                }

                if (toSet2.HasValue)
                {
                    funcB = toSet2.Value;
                }

            
                var forcedExit = false;
                if (!toSet.HasValue || !toSet2.HasValue)
                {
                    // Cannot calculate the value. Get the nearest value and exit.
                    forcedExit = true;
                }

                var result = GetNearestOrMin(funcA, funcB, isolevel);

                if (result)
                {
                    curB = calcB;

                    // This will exit from the function.
                    if(forcedExit)
                    {
                        curA = curB;
                    }

                    calculated = Calculated.B;
                }
                else
                {
                    curA = calcA;

                    if (forcedExit)
                    {
                        curB = curA;
                    }

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