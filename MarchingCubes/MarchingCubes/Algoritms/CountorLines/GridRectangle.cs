using System;
using System.Collections.Generic;
using GradientDescent.CommonTypes;
using GradientDescent.GraphicTypes;
namespace GradientDescent.Algoritms.CountorLines
{
    public class GridRectangle : ICloneable
    {
        /// <summary>
        /// Right=0,Top=1,Left=2,Bottom=3
        /// </summary>
        public GridRectangle(params GridLine[] lines)
        {
            if (lines.Length != 4)
                throw new ArgumentException("rectangles count have to be 4");
            this.Lines = lines;
        }

        public bool IsContainsCountorLine()
        {
            foreach (var line in Lines)
            {
                if (line.IsContainsCountorLine)
                    return true;
            }
            return false;
        }

        public void CleanUpAttacments()
        {
            foreach (var line in Lines)
            {
                line.IsAttached = false;
            }
        }
        public static int Right = 0;
        public static int Top = 1;
        public static int Left = 2;
        public static int Bottom = 3;

        /// <summary>
        /// Right=0,Top=1,Left=2,Bottom=3
        /// </summary>
        public GridLine[] Lines { get; set; }
        /// <summary>
        /// index 0
        /// </summary>
        public GridLine RightLine { get { return Lines[0]; } set { Lines[0] = value; } }
        /// <summary>
        /// index 1
        /// </summary>
        public GridLine TopLine { get { return Lines[1]; } set { Lines[1] = value; } }
        /// <summary>
        /// index 2
        /// </summary>
        public GridLine LeftLine { get { return Lines[2]; } set { Lines[2] = value; } }
        /// <summary>
        /// index 3
        /// </summary>
        public GridLine BottomLine { get { return Lines[3]; } set { Lines[3] = value; } }
        public GridRectangle()
        {
            Lines = new GridLine[4];
        }
        /// <summary>
        /// lefttop,righttop,leftbottom,rightbottom
        /// </summary>
        public Arguments[] Vertices
        {
            get
            {
                Arguments[] args = new Arguments[4];
                args[0] = Lines[1].Point1;
                args[1] = Lines[1].Point2;
                args[2] = Lines[3].Point1;
                args[3] = Lines[3].Point2;
                return args;
            }
        }
        #region ICloneable Members

        public object Clone()
        {
            return CloneGridRectangle();
        }

        public GridRectangle CloneGridRectangle()
        {
            List<GridLine> toReturn = new List<GridLine>();
            foreach (var line in Lines)
            {
                toReturn.Add(line.CloneLine());
            }
            return new GridRectangle(toReturn.ToArray());
        }

        public void SetZ(double z)
        {
            foreach (var line in Lines)
            {
                line.ChangeZ(z);
            }
        }
        #endregion

    }
    public enum GridRectangleSide
    {
        Left = 0, Top = 1, Right = 2, Bottom = 3
    }
}


