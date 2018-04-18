using MarchingCubes.CommonTypes;
using MarchingCubes.GraphicTypes;
using MarchingCubes.Algoritms.CountorLines;
using System;

namespace MarchingCubes.Algoritms.MarchingCubes
{
    public class GridLine : Line//, ICloneable
    {
        public GridLine(Point point1, Point point2, int index = 0)
            : base(point1, point2)
        {
            this.Index = index;
            IsAnalyzed = false;
            IsContainsCountorLine = false;
            CountorData = new GridCountorsList();
        }

        public int Index { get; set; }
        public GridLine() : base() { }

        public bool IsAnalyzed { get; set; }

        public bool IsContainsCountorLine { get; set; }

        /// <summary>
        /// Class for multiple calculation
        /// </summary>
        public GridCountorsList CountorData { get; set; }

        public bool IsAttached { get; set; }

        public object Clone()
        {
            return CloneLine();
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


