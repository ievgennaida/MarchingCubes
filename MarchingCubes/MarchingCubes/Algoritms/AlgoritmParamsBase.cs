using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GradientDescent.CommonTypes;

namespace GradientDescent.Algoritms
{
    public class AlgoritmParamsBase
    {
        protected Arguments startPoint;

        public Arguments StartPoint
        {
            get { return startPoint; }
            set { startPoint = value; }
        }

        private double accuracyEpsilon = 0.01;

        public double AccuracyEpsilon
        {
            get { return accuracyEpsilon; }
            set { accuracyEpsilon = value; }
        }
    }
}
