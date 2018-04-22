using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using GradientDescent.CommonTypes;
using GradientDescentLogic.Algoritms.CountorLines;

namespace GradientDescent.CommonTypes
{
    public class VectorHelper
    {
        public static Vector3D CalcNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(
                p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(
                p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);

            return Vector3D.CrossProduct(v0, v1);
        }
        public static Vector3D CalcNormal(Arguments p0, Arguments p1, Arguments p2)
        {
            Vector3D v0 = new Vector3D(
                p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(
                p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);

            return Vector3D.CrossProduct(v0, v1);
        }

        public static Vector3D CalcNormal(GridTriangle triangle)
        {
            return CalcNormal(triangle.Point1, triangle.Point2, triangle.Point3);
        }
        public static void AddNormals(GridTriangle triangle, Vector3D normal)
        {
           // triangle.Point1.NormalVector = Vector3D.Add(triangle.Point1.NormalVector, normal);
           // triangle.Point2.NormalVector = Vector3D.Add(triangle.Point2.NormalVector, normal);
           // triangle.Point3.NormalVector = Vector3D.Add(triangle.Point3.NormalVector, normal);
        }
    }
}
