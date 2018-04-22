using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MarchingCubes.CommonTypes
{
    [DebuggerDisplay("P {X}{Y}{Z}")]
    public struct Point
    {
        public Point(double x, double y, double z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override bool Equals(object ob)
        {
            if (ob is Point)
            {
                var c = (Point)ob;
                var isEquals = X == c.X && Y == c.Y && Z == c.Z; ;
                return isEquals;
            }
            else
            {
                return false;
            }
        }
    }
}
