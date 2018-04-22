using MarchingCubes.CommonTypes;
using System;

namespace MarchingCubes.Algoritms.MarchingCubes
{
    public class GridLine 
    {
        public GridLine() 
        {
        }

        public GridLine(Point point1, Point point2, int axissIndex = 0)
        {
            this.Point1 = point1;
            this.Point2 = point2;
            this.AxissIndex = axissIndex;
            IsAnalyzed = false;
            HasIsoLine = false;
        }

        public int AxissIndex { get; set; }


        public bool IsAnalyzed { get; set; }

        public bool HasIsoLine { get; set; }

        public object Clone()
        {
            return CloneLine();
        }

        public Point Point1 { get; set; }

        public Point Point2 { get; set; }

        public Point IsoPoint { get; set; }

        public static double Lenght(float x1, float x2, float y1, float y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public GridLine CloneLine()
        {
            return new GridLine()
            {
                Point1 = this.Point1,
                Point2 = this.Point2
            };
        }
    }
}


