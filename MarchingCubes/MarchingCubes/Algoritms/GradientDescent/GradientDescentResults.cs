using GradientDescent.CommonTypes;
using GradientDescentLogic.CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GradientDescent.Algoritms.GradientDescent
{
    public class GradientDescentResults : List<GradientResultItem>
    {
        public static double AdditionalRegionPecents = 12;

        public Region3D GetRegion()
        {
            return Arguments.GetRegion(AdditionalRegionPecents, this.Select(p => p.Approximation).ToList());
        }

        public String Function { get; set; }
        public GradientDescentParams GradientDescentParams { get; set; }
    }
}
