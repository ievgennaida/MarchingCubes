using MarchingCubes.Algoritms.CountorLines;
using MarchingCubes.GraphicTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarchingCubes.Algoritms.MarchingCubes
{
    public class MarchingCubesResults
    {
        public MarchingCubesResults()
        {
            Triangles = new List<Triangle>();
        }

        public double Value { get; set; }
        public List<Triangle> Triangles { get; set; }

        public List<Line> GetWiredResults()
        {
            var wires = new List<Line>();
            foreach (var triangle in Triangles)
            {
                if (triangle.IsInvalid)
                    continue;
                //wires.Add(new Line(triangle.Point1, triangle.Point2));
                //wires.Add(new Line(triangle.Point2, triangle.Point3));
                //wires.Add(new Line(triangle.Point3, triangle.Point1));
            }
            return wires;
        }
    }
}
