using GradientDescent.CommonTypes;
using GradientDescent.GraphicTypes;
using GradientDescentLogic.Algoritms.CountorLines;
using System;

namespace GradientDescent.Algoritms.CountorLines
{
    public class GridLine : Line, ICloneable
    {
        public GridLine(Arguments point1, Arguments point2)
            : base(point1, point2)
        {
            IsAnalyzed = false;
            IsContainsCountorLine = false;
            CountorData = new GridCountorsList();
        }
        public int Index { get; set; }
        public GridLine() : base() { }

        public bool FoundMinium
        {
            get { return MiniumF != null || Minium != null; }
            set
            {
                if (value)
                    throw new InvalidOperationException("Found minium can be set only to false");
                else
                {
                    Minium = null;
                    MiniumF = null;
                }
            }
        }
        public void ChangeZ(double z)
        {
            base.Point1.Z = z;
            base.Point2.Z = z;
        }

        public bool IsAnalyzed { get; set; }

        public double CalculatedValue1 { get; set; }

        public double CalculatedValue2 { get; set; }

        /// <summary>
        /// for real value
        /// </summary>
        public Arguments Minium { get; set; }

        /// <summary>
        /// for screen point
        /// </summary>
        public Arguments MiniumF { get; set; }

        public bool IsContainsCountorLine { get; set; }

        /// <summary>
        /// Class for multiple calculation
        /// </summary>
        public GridCountorsList CountorData { get; set; }

        public bool IsAttached { get; set; }

        public object Clone()
        {
            return CloneLine();
        }

        public GridLine CloneLine()
        {
            return new GridLine()
            {
                Point1 = Point1.CloneArguments(),
                Point2 = Point2.CloneArguments()
            };
        }
    }
}


