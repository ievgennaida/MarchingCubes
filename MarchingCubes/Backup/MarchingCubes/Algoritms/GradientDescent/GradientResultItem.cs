using MarchingCubes.CommonTypes;

namespace MarchingCubes.Algoritms.GradientDescent
{
    public class GradientResultItem
    {
        public Arguments Approximation { get; set; }
        public Arguments AntiGradient { get; set; }
        public double FunctionValue { get; set; }
        public double AntiGradModule { get; set; }
        public double Accuracy { get; set; }
        public bool AccuracyCondition { get; set; }
        public double curB { get; set; }
        public int Index { get; set; }
    }
}
