using MarchingCubes.CommonTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarchingCubes.Algoritms.MarchingCubes
{
    public class Triangle
    {
        public Triangle(Point p1, Point p2, Point p3)
        {
            this.Point1 = p1;
            this.Point2 = p2;
            this.Point3 = p3;
        }

        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public Point Point3 { get; set; }

        public Point GetNormal()
        {
            var p0 = this.Point1;
            var p1 = this.Point2;
            var p2 = this.Point3;

            var v0 = new Point(
                p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            var v1 = new Point(
                p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);

            // CrossProduct
            var x = v0.Y * v1.Z - v0.Z * v1.Y;
            var y = v0.Z * v1.X - v0.X * v1.Z;
            var z = v0.X * v1.Y - v0.Y * v1.X;

            var result = new Point(x, y, z);
            return result;
        }
    }
}
