using MarchingCubes.CommonTypes;
using System;

namespace MarchingCubes.GraphicTypes
{
    public class Line
    {
        public Line()
            : this(new Point(), new Point())
        {
        }

        public Line(Point point1, Point point2)
        {
            this.Point1 = point1;
            this.Point2 = point2;
        }

        public Point Point1 { get; set; }

        public Point Point2 { get; set; }
        //public Point GetCenterPoint()
        //{
        //    if (center!=null)
        //        return center;
        //    var args=new Arguments();
        //    for (int index = 0; index < Dimension; index++)
        //    {
        //        args.Add((point1[index].Value + point2[index].Value) / 2);
        //    }
        //    args.SetNames();
        //    center = args;
        //    return center;
        //}

        //public double Lenght()
        //{
        //    double result = 0;
        //    for(int index=0;index<Dimension;index++)
        //    {
        //        result += Math.Pow((point2[index].Value - point1[index].Value), 2); 
        //    }
        //    return Math.Sqrt(result);
        //}

        public static double Lenght(float x1, float x2, float y1, float y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public override string ToString()
        {
            return Point1.ToString() + " " + Point2.ToString();
        }
    }
}