using GradientDescent.CommonTypes;

namespace GradientDescent.Algoritms
{
    public abstract class AlgoritmBase
    {
        protected FunctionHolder function;

        protected Arguments startPoint;

        public AlgoritmBase() { AccuracyEpsilon = 0.05; }

        public AlgoritmBase(FunctionHolder function, Arguments startPoint):this()
        {
            this.function = function;
            this.StartPoint = startPoint;
        }

        public AlgoritmBase(FunctionHolder function, Arguments startPoint, double accuracyEpsilon)
        {
            this.function = function;
            this.StartPoint = startPoint;
            this.AccuracyEpsilon = accuracyEpsilon;
        }

        public double AccuracyEpsilon { get; set; }

        public Arguments StartPoint { get; set; }
    }
}