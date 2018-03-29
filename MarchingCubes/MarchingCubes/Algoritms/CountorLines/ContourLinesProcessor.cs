using GradientDescent.CommonTypes;
using GradientDescent.GraphicTypes;
using GradientDescentLogic;
using System.Collections.Generic;

namespace GradientDescent.Algoritms.CountorLines
{
    /// <summary>
    /// Використані алгоритми:
    /// Marching Cubes та
    /// Adaptive Marching Cubes
    /// 2D випадок
    /// </summary>
    public class ContourLinesProcessor
    {
        FunctionHolder function;
        CoordinatesScaler scaler;
        int meshSize;
        List<Line> cashedLines = null;
        public List<GridRectangle> rectangles = null;

        public ContourLinesProcessor(FunctionHolder function, CoordinatesScaler scaler, int meshSize)
        {
            this.function = function;
            this.scaler = scaler;
            this.meshSize = meshSize;
            scaler.RectanglesChanged += new SizeChanged(scaler_RectanglesChanged);
            BuildGridLines();
        }

        void scaler_RectanglesChanged(CoordinatesScaler scaler, RectangleSizeChangedEventArgs mode)
        {
            if (mode.Mode == CSChangedMode.ScreenRectangle)
            {
                BuildGridLines();
            }
        }

        public ContourLinesProcessor(FunctionHolder function, CoordinatesScaler scaler) : this(function, scaler, 5) { }



        double valueToDraw;

        public int MeshSize
        {
            get { return meshSize; }
            set
            {
                meshSize = value;
                MeshSizeChanged();
            }
        }

        private void MeshSizeChanged()
        {
            cashedLines = null;
            BuildGridLines();
        }


        //public List<PointF> DrawSimpleAlgoritm(Arguments point)
        //{
        //    valueToDraw = function.Calculate(point);
        //    return DrawSimpleAlgoritm(valueToDraw);
        //}

        //public List<PointF> DrawSimpleAlgoritmByArguments(Arguments value)
        //{
        //    return DrawSimpleAlgoritm(function.Calculate(value));
        //}

        //public List<PointF> DrawSimpleAlgoritm(double value)
        //{
        //    List<PointF> points = new List<PointF>();

        //    for (var y = scaler.ScreenRegion.MinY; y <= scaler.ScreenRegion.MinY + scaler.ScreenRegion.Height; y = y + 1)
        //    {
        //        for (var x = scaler.ScreenRegion.MinX; x <= scaler.ScreenRegion.MinX + scaler.ScreenRegion.Width; x = x + 1)
        //        {
        //            var result = function.Calculate(scaler.ToAxisPoint(x, y));
        //            if (result + 0.01 >= value && value >= result - 0.01)
        //                points.Add(new PointF((float)x, (float)y));
        //        }
        //    }
        //    return points;
        //}

        public List<Line> ShowGrid()
        {
            cashedLines = new List<Line>();

            bool drawY = scaler.ScreenRegion.MinY <= scaler.ScreenRegion.MinY + scaler.ScreenRegion.Height;
            for (var y = scaler.ScreenRegion.MinY; drawY; y = y + meshSize)
            {
                if (drawY)
                {
                    drawY = y <= scaler.ScreenRegion.MinY + scaler.ScreenRegion.Height;
                    if (drawY == false)
                    {
                        y = scaler.ScreenRegion.MinY + scaler.ScreenRegion.Height;
                    }
                }
                cashedLines.Add(new Line(new Arguments(scaler.ScreenRegion.MinX, y), new Arguments(scaler.ScreenRegion.MinX + scaler.ScreenRegion.Width, y)));
            }
            bool drawX = scaler.ScreenRegion.MinX <= scaler.ScreenRegion.MinX + scaler.ScreenRegion.Width;
            for (var x = scaler.ScreenRegion.MinX; drawX; x = x + meshSize)
            {
                if (drawX)
                {
                    drawX = x <= scaler.ScreenRegion.MinX + scaler.ScreenRegion.Width;
                    if (drawX == false)
                    {
                        x = scaler.ScreenRegion.MinX + scaler.ScreenRegion.Width;
                    }
                }
                cashedLines.Add(new Line(new Arguments(x, scaler.ScreenRegion.MinY), new Arguments(x, scaler.ScreenRegion.MinY + scaler.ScreenRegion.Height)));
            }

            return cashedLines;
        }

        public List<GridRectangle> BuildGridLines()
        {
            var vertical = new List<GridLine>();
            var horisontal = new List<GridLine>();
            int linesInRow = 0;
            int linesInColumn = 0;
            rectangles = new List<GridRectangle>();
            //int linesInCycle = 0;
            var width = scaler.ScreenRegion.MinX + scaler.ScreenRegion.MaxX;
            var height = scaler.ScreenRegion.MinY + scaler.ScreenRegion.MaxY;
            double yMeshSize = meshSize;
            double xMeshSize = meshSize;
            bool calculateLines = true;
            for (var y = scaler.ScreenRegion.MinY; y <= height; y = y + yMeshSize)
            {
                var destY = y + meshSize;
                if (y != height)
                {
                    bool yOutOfBorder = y + meshSize > scaler.ScreenRegion.MinY + scaler.ScreenRegion.MaxY;
                    if (yOutOfBorder)
                    {
                        yMeshSize = height - y;
                    }
                    destY = yOutOfBorder ? scaler.ScreenRegion.MaxY : y + meshSize;
                }

                bool xOutOfBorder = false;
                for (var x = scaler.ScreenRegion.MinX; x <= width; x = x + xMeshSize)
                {
                    if (x != width)
                    {
                        xOutOfBorder = (x + meshSize > width);
                        if (xOutOfBorder)
                        {
                            xMeshSize = width - x;
                        }
                        var destX = xOutOfBorder ? scaler.ScreenRegion.MaxX : x + meshSize;

                        //horisontal
                        horisontal.Add(new GridLine(new Arguments(x, y), new Arguments(destX, y)));

                        if (calculateLines)
                            linesInRow++;

                        //rectange generation. if we have not first we
                        //can generate new rectange
                        if (y != scaler.ScreenRegion.MinY)
                        {

                            rectangles.Add(
                                new GridRectangle(vertical[vertical.Count - linesInColumn + 1]/*right*/,
                                                  horisontal[horisontal.Count - linesInRow - 1]/*top*/,
                                                  vertical[vertical.Count - linesInColumn]/*left*/,
                                                  horisontal[horisontal.Count - 1]/*bottom*/));
                        }
                    }
                    if (y != height)
                    {
                        if (calculateLines)
                            linesInColumn++;

                        //vertical
                        vertical.Add(new GridLine(new Arguments(x, y), new Arguments(x, destY)));
                    }
                    else
                    {
                        //need to get indexes for last row.
                        linesInColumn--;
                    }
                }
                calculateLines = false;
                xMeshSize = meshSize;
            }
            return rectangles;
        }

        private void ClearAnalyzationData()
        {
            foreach (var rec in rectangles)
            {
                foreach (var line in rec.Lines)
                {
                    line.IsAnalyzed = false;
                    line.IsContainsCountorLine = false;
                }
            }
        }

        private void AnalyzeRectangles(List<GridRectangle> rectangles, double value)
        {
            foreach (var rec in rectangles)
            {
                foreach (var line in rec.Lines)
                {
                    if (!line.IsAnalyzed)
                    {
                        line.AxisPoint1 = scaler.ToAxisPoint(line.Point1);
                        line.AxisPoint2 = scaler.ToAxisPoint(line.Point2);
                        line.CalculatedValue1 = function.Calculate(line.AxisPoint1);
                        line.CalculatedValue2 = function.Calculate(line.AxisPoint2);
                        line.IsAnalyzed = true;
                        line.FoundMinium = false;
                        if (line.CalculatedValue1 <= value != line.CalculatedValue2 <= value)//Через линию проходит линия уровня
                        {
                            line.IsContainsCountorLine = true;
                        }
                    }
                }
            }
        }
        public List<Line> CreateFastContourLine(Arguments point)
        {
            return CreateContourLine(point, false);
        }

        public List<Line> CreateFastContourLine(double value)
        {
            return CreateContourLine(value, false);
        }

        public List<Line> CreateAdaptiveContourLine(Arguments point)
        {
            return CreateContourLine(point, true);
        }

        public List<Line> CreateAdaptiveContourLine(double value)
        {
            return CreateContourLine(value, true);
        }

        public List<Line> CreateContourLine(Arguments point, bool isAdaptive)
        {
            return CreateContourLine(function.Calculate(point), isAdaptive);
        }

        public List<Line> CreateContourLine(double value, bool isAdaptive)
        {
            var goldenRatioAlgoritm = new GoldenSectionSearch(this.function);
            goldenRatioAlgoritm.AccuracyEpsilon = 0.005;
            AnalyzeRectangles(rectangles, value);
            List<Line> lines = new List<Line>();

            foreach (var rec in rectangles)
            {
                if (rec.IsContainsCountorLine())
                {
                    foreach (var line in rec.Lines)
                    {
                        if (!line.IsAttached && line.IsContainsCountorLine)
                        {
                            foreach (var line2 in rec.Lines)
                            {
                                if (!line2.IsAttached && line2.IsContainsCountorLine)
                                    if (line != line2)
                                    {
                                        if (isAdaptive)
                                        {
                                            if (!line.FoundMinium)
                                            {
                                                line.MiniumF = GetLineMinium(goldenRatioAlgoritm, line, value);
                                                line.Minium = scaler.ToScreenPoint(line.MiniumF);
                                            }

                                            if (!line2.FoundMinium)
                                            {
                                                line2.MiniumF = GetLineMinium(goldenRatioAlgoritm, line2, value);
                                                line2.Minium = scaler.ToScreenPoint(line2.MiniumF);
                                            }
                                            lines.Add(new Line(line.Minium, line2.Minium, line.MiniumF, line2.MiniumF));
                                        }
                                        else
                                        {
                                            lines.Add(new Line(line.GetCenterPoint(), line2.GetCenterPoint()));
                                        }
                                        line2.IsAttached = line.IsAttached = true;
                                    }
                            }
                        }
                    }
                    rec.CleanUpAttacments();
                }
            }
            ClearAnalyzationData();
            return lines;
        }

        public int GetIndexToOptimize(Line line)
        {
            if (line.Point1.X != line.Point2.X)
                return AxissConsts.X;
            else if (line.Point1.Y != line.Point2.Y)
                return AxissConsts.Y;

            return AxissConsts.Z;
        }

        public Arguments GetLineMinium(GoldenSectionSearch algoritm, Line line, double minium)
        {
            return algoritm.GetMinium(line.AxisPoint1, line.AxisPoint2, line.AxisPoint1, GetIndexToOptimize(line), true, minium);
        }
    }
}


