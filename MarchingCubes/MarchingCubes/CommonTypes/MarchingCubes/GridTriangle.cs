using MarchingCubes.CommonTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarchingCubes.Algoritms.MarchingCubes
{
    public class Triangle
    {
        public Arguments Point1 { get; set; }
        public Arguments Point2 { get; set; }
        public Arguments Point3 { get; set; }
        public Boolean IsInvalid { get { return (Point1 == null || Point2 == null || Point3 == null); } }
    }
}
