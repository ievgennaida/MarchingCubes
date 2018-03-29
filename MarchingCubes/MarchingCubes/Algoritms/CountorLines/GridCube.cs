using GradientDescent.CommonTypes;
using System;
using System.Collections.Generic;
namespace GradientDescent.Algoritms.CountorLines
{
    /// <summary>
    /// Contains 6 GridRectangles
    /// Куб для алгоритму біжучих кубиків
    /// </summary>
    public class GridCube
    {
        public GridCube()
        {
            Rectangles = new GridRectangle[6];
        }

        public GridCube(params GridRectangle[] rectangles)
        {
            if (this.Rectangles.Length != 6)
                throw new ArgumentException("rectangles count have to be 6");
            this.Rectangles = rectangles;
        }
        /// <summary>
        ///  Front = 0,Left = 1,Bottom = 2,Right = 3,Top = 4,Back = 5
        /// </summary>
        public GridRectangle[] Rectangles { get; set; }
        public GridRectangle FrontRectangle { get { return Rectangles[0]; } set { Rectangles[0] = value; } }
        public GridRectangle LeftRectangle { get { return Rectangles[1]; } set { Rectangles[1] = value; } }
        public GridRectangle BottomRectangle { get { return Rectangles[2]; } set { Rectangles[2] = value; } }
        public GridRectangle RightRectangle { get { return Rectangles[3]; } set { Rectangles[3] = value; } }
        public GridRectangle TopRectangle { get { return Rectangles[4]; } set { Rectangles[4] = value; } }
        public GridRectangle BackRectangle { get { return Rectangles[5]; } set { Rectangles[5] = value; } }

        public Arguments GetCubeCenter()
        {
            var args = Arguments.CreateEmptyArgs(3);
            args[0].Value = Rectangles[(int)GridCubeSides.Front].Lines[(int)GridRectangleSide.Top].GetCenterPoint()[0].Value;
            args[1].Value = Rectangles[(int)GridCubeSides.Front].Lines[(int)GridRectangleSide.Left].GetCenterPoint()[1].Value;
            args[2].Value = Rectangles[(int)GridCubeSides.Bottom].Lines[(int)GridRectangleSide.Top].GetCenterPoint()[2].Value;
            return args;
        }

        /// <summary>
        /// Get Lines(Edges)
        /// </summary>
        /// <returns></returns>
        public List<GridLine> GetEdges()
        {
            var results = new List<GridLine>();
            var line=this.BottomRectangle.LeftLine;
            results.Add(Rectangles[(int)GridCubeSides.Back].Lines[(int)GridRectangleSide.Bottom]);//0
            results.Add(Rectangles[(int)GridCubeSides.Right].Lines[(int)GridRectangleSide.Bottom]);//1
            results.Add(Rectangles[(int)GridCubeSides.Front].Lines[(int)GridRectangleSide.Bottom]);//2
            results.Add(Rectangles[(int)GridCubeSides.Left].Lines[(int)GridRectangleSide.Bottom]);//3

            results.Add(Rectangles[(int)GridCubeSides.Back].Lines[(int)GridRectangleSide.Top]);//4
            results.Add(Rectangles[(int)GridCubeSides.Right].Lines[(int)GridRectangleSide.Top]);//5
            results.Add(Rectangles[(int)GridCubeSides.Front].Lines[(int)GridRectangleSide.Top]);//6
            results.Add(Rectangles[(int)GridCubeSides.Left].Lines[(int)GridRectangleSide.Top]);//7

            results.Add(Rectangles[(int)GridCubeSides.Back].Lines[(int)GridRectangleSide.Right]);//8
            results.Add(Rectangles[(int)GridCubeSides.Back].Lines[(int)GridRectangleSide.Left]);//9

            results.Add(Rectangles[(int)GridCubeSides.Front].Lines[(int)GridRectangleSide.Left]);//10
            results.Add(Rectangles[(int)GridCubeSides.Front].Lines[(int)GridRectangleSide.Right]);//11
            return results;
        }

        /// <summary>
        /// Отримати значення функції в вершинах
        /// back. bottom 0 right 1 top
        /// </summary>
        /// <returns></returns>
        public List<double> GetVertexValues()
        {
            var points = new List<double>()
            {
                Rectangles[(int)GridCubeSides.Back].BottomLine.CalculatedValue1,
                Rectangles[(int)GridCubeSides.Back].BottomLine.CalculatedValue2,
                Rectangles[(int)GridCubeSides.Front].BottomLine.CalculatedValue2,
                Rectangles[(int)GridCubeSides.Front].BottomLine.CalculatedValue1,
                Rectangles[(int)GridCubeSides.Back].TopLine.CalculatedValue1,
                Rectangles[(int)GridCubeSides.Back].TopLine.CalculatedValue2,
                Rectangles[(int)GridCubeSides.Front].TopLine.CalculatedValue2,
                Rectangles[(int)GridCubeSides.Front].TopLine.CalculatedValue1

            };
            return points;
        }
        public int LastCubeIndex { get; set; }
        /// <summary>
        /// Get special index of cube isolevel for marching cubes algoritm
        /// </summary>
        /// <returns></returns>
        public int GetCubeIndex(double isolevel)
        {
            var points = GetVertexValues();
            int cubeindex = 0;
            if (points[0] < isolevel) cubeindex |= 1;
            if (points[1] < isolevel) cubeindex |= 2;
            if (points[2] < isolevel) cubeindex |= 4;
            if (points[3] < isolevel) cubeindex |= 8;
            if (points[4] < isolevel) cubeindex |= 16;
            if (points[5] < isolevel) cubeindex |= 32;
            if (points[6] < isolevel) cubeindex |= 64;
            if (points[7] < isolevel) cubeindex |= 128;
            LastCubeIndex = cubeindex;
            return cubeindex;
        }
        /// <summary>
        /// Отримати лінії, які мають лінію рівня
        /// </summary>
        /// <returns></returns>
        public List<GridLine> GetLineToInterpolate()
        {
            int lineCount = 0;
            List<GridLine> linesToInterpolate = new List<GridLine>();
            foreach (var rectangle in Rectangles)
            {
                foreach (var line in rectangle.Lines)
                {
                    if (line.IsContainsCountorLine)
                    {
                        line.Index = lineCount;
                        linesToInterpolate.Add(line);
                    }
                    lineCount++;
                }
            }
            return linesToInterpolate;
        }
    }

    public enum GridCubeSides
    {
        Front = 0,
        Left = 1,
        Bottom = 2,
        Right = 3,
        Top = 4,
        Back = 5
    }
}


