using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GradientDescent.CommonTypes;

namespace GradientDescent.Algoritms.GradientDescent
{
    public class GradientDescentParams : AlgoritmParamsBase
    {
        public double Beta { get; set; } = 0.5;
        public double Epsilon { get; set; } = 0.5;
        public double Alpha { get; set; } = 0.5;
        public double M { get; set; } = 1.5;
        public bool IsModification { get; set; } = true;

        public static GradientDescentParams DefaultParams(int dimension)
        {
                return new GradientDescentParams()
                {
                    StartPoint = Arguments.CreateEmptyArgs(dimension)
                };
        }
    }
}
