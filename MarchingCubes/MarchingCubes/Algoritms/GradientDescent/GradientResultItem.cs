using MarchingCubes.CommonTypes;

namespace MarchingCubes.Algoritms.GradientDescent
{
    public class GradientResultItem
    {
        /// <summary>
        /// Наближення
        /// </summary>
        public Arguments Approximation { get; set; }

        /// <summary>
        /// Вектор антиградієнт
        /// </summary>
        public Arguments AntiGradient { get; set; }
        public double FunctionValue { get; set; }
        public double AntiGradModule { get; set; }
        public double Accuracy { get; set; }
        public bool AccuracyCondition { get; set; }
        public double curB { get; set; }
        public int Index { get; set; }
    }
}
