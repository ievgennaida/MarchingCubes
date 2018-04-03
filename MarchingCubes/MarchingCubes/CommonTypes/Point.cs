using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MarchingCubes.CommonTypes
{
    [DebuggerDisplay("P {X}{Y}{Z}")]
    public class Point
    {
        public Point()
        {

        }

        public Point(double x, double y, double z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
