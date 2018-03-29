using GradientDescent.CommonTypes;
using System;

namespace GradientDescent.GraphicTypes
{
    public class Line
    {
        private Arguments center;

        private Arguments centerf;
        private Arguments point1;
        private Arguments point2;

        public Line()
        {
            point1 = new Arguments();
            point2 = new Arguments();
            center = null;
            centerf = null;
        }

        public Line(Arguments point1, Arguments point2, Arguments axisPoint1 = null, Arguments axisPoint2 = null)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.AxisPoint1 = axisPoint1;
            this.AxisPoint2 = axisPoint2;
        }

        /// <summary>
        /// Show in what dimension points.
        /// </summary>
        public int Dimension
        {
            get
            {
                if (Point1.Count != Point2.Count)
                    throw new InvalidOperationException("Cannot determine Dimension because point1 and point2 have diferent Dimension");
                return Point1.Count; 
            }
        }

        public Arguments AxisPoint1 { get; set; }

        public Arguments AxisPoint2 { get; set; }

        public Arguments Point1
        {
            get { return point1; }
            set
            {
                point1 = value;
                center = null;
                centerf = null;
            }
        }

        public Arguments Point2
        {
            get { return point2; }
            set
            {
                point2 = value;
                center = null;
                centerf = null;
            }
        }

        public Arguments GetCenterPoint()
        {
            if (center!=null)
                return center;
            var args=new Arguments();
            for (int index = 0; index < Dimension; index++)
            {
                args.Add((point1[index].Value + point2[index].Value) / 2);
            }
            args.SetNames();
            center = args;
            return center;
        }

        public double Lenght()
        {
            double result = 0;
            for(int index=0;index<Dimension;index++)
            {
                result += Math.Pow((point2[index].Value - point1[index].Value), 2); 
            }
            return Math.Sqrt(result);
        }

        public static double Lenght(float x1, float x2, float y1, float y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public override string ToString()
        {
            return point1.ToString() + " " + point2.ToString();
        }
    }
}