using MarchingCubes;
using MarchingCubes.CommonTypes;
using System;

namespace GradientDescentWPF.ViewModels
{
    public class FunctionData
    {
        public MarchingFunction Function { get; set; }
        public string Description { get; set; }
        public string FullDescription { get; set; }

        public bool IsFucnctionSet
        {
            get { return Function == null; }
        }

        public Region3D Region { get; set; }
        public double CountorLine { get; set; }
        public double Step { get; set; }
    }
}
