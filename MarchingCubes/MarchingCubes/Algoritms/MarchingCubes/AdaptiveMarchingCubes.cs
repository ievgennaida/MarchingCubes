using MarchingCubes.CommonTypes;
using MarchingCubes.GraphicTypes;
using System;
using System.Collections.Generic;

namespace MarchingCubes.Algoritms.CountorLines
{
    /// <summary>
    /// Використані алгоритми:
    /// Marching Cubes та
    /// Adaptive Marching Cubes
    /// 3D випадок
    /// </summary>
    public class AdaptiveMarchingCubes
    {
        FunctionHolder function;
        // double meshSize;
        List<Line> cashedLines = null;
        public GridRectangle[][] rectangles = null;
        public List<GridCube> cubes = null;

        public AdaptiveMarchingCubes(FunctionHolder function)
        {
            this.function = function;
            //this.meshSize = meshSize;
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

        public List<Line> ShowGrid(Region3D region, double meshSize)
        {
            cashedLines = new List<Line>();
            bool drawZ = region.MinZ <= region.MinZ + region.Depth;
            for (var z = region.MinZ; drawZ; z = z + meshSize)
            {
                if (drawZ)
                {
                    drawZ = z < region.MinZ + region.Depth;
                    if (!drawZ)
                    {
                        z = region.MaxZ;
                    }
                }
                bool drawY = region.MinY <= region.MinY + region.Height;
                for (var y = region.MinY; drawY; y = y + meshSize)
                {
                    if (drawY)
                    {
                        drawY = y <= region.MinY + region.Height;
                        if (drawY == false)
                        {
                            y = region.MinY + region.Height;
                        }
                    }
                    cashedLines.Add(new Line(new Arguments(region.MinX, y, z), new Arguments(region.MinX + region.Width, y, z)));
                }
                bool drawX = region.MinX <= region.MinX + region.Width;
                for (var x = region.MinX; drawX; x = x + meshSize)
                {
                    if (drawX)
                    {
                        drawX = x <= region.MinX + region.Width;
                        if (drawX == false)
                        {
                            x = region.MinX + region.Width;
                        }
                    }
                    cashedLines.Add(new Line(new Arguments(x, region.MinY, z), new Arguments(x, region.MinY + region.Height, z)));
                }
            }
            return cashedLines;
        }

        /// <summary>
        /// Cube 
        /// first side = 0
        /// left 1, bottom 2, right 3 top 4
        /// back = 5
        /// </summary>
        /// <returns></returns>
        public List<GridCube> BuildGrid(Region3D region, double step)
        {
            var toReturn = new List<GridCube>();

            // Rectangle indexes: Left 0, Top 1, Right 2, Bottom 3
            // Cube indexes: Front 0 Left 1 Bottom 2, 

            var totalRows = 0;
            var totalColumns = 0;

            var column = 0;
            var prevColumn = 0;
            for (var y = region.MinY; y < region.MaxY;)
            {
                var isLastColumn = (y + step) > region.MaxY;
                var isFirstY = column == 0;
                var nextYValue = isLastColumn ? region.MaxY : y + step;

                var depth = 0;
                var prevDepth = 0;
                var currentZcolumn = 0;
                for (var z = region.MinZ; z < region.MaxZ;)
                {
                    var isLastDepth = (z + step) > region.MaxZ;
                    var isFirstZ = depth == 0;
                    var nextZValue = isLastDepth ? region.MaxZ : z + step;
                    currentZcolumn++;

                    var row = 0;
                    var prevX = region.MinX;

                    var currentRow = 0;
                    for (var x = region.MinX; x < region.MaxX;)
                    {
                        var isLastX = (x + step) > region.MaxX;
                        var isFirstX = row == 0;
                        var nextXValue = isLastX ? region.MaxX : x + step;

                        if (!isLastColumn && !isLastX && !isLastDepth || (isFirstX && isLastX))
                        {
                            GridCube prevXCube = null;
                            if (!isFirstX)
                            {
                                prevXCube = toReturn[toReturn.Count - 1];
                            }

                            GridCube prevYCube = null;
                            if (!isFirstY)
                            {
                                prevYCube = toReturn[currentRow - totalRows - totalColumns];
                            }

                            GridCube prevZCube = null;
                            if (!isFirstZ)
                            {
                                prevZCube = toReturn[currentRow - totalRows];
                            }

                            var newCube = new GridCube();

                            // Dont create the duplicate objects. 
                            var edges = new List<GridLine>();
                            //0
                            var edge = MergeEdge(prevYCube, 4, prevZCube, 2);
                            edge = edge == null ? new GridLine(new Arguments(x, y, z)/*0*/, new Arguments(nextXValue, y, z)/*1*/) : edge;
                            edges.Add(edge);
                            newCube.Vertex[0] = edge.Point1;
                            newCube.Vertex[1] = edge.Point2;

                            //1
                            edge = MergeEdge(prevYCube, 5, prevXCube, 3);
                            edge = edge == null ? new GridLine(new Arguments(nextXValue, y, z)/*1*/, new Arguments(nextXValue, y, nextZValue)/*2*/) : edge;
                            edges.Add(edge);

                            //2
                            edge = MergeEdge(prevYCube, 6, prevZCube, 0);
                            edge = edge == null ? new GridLine(new Arguments(nextXValue, y, nextZValue)/*2*/, new Arguments(x, y, nextZValue)/*3*/) : edge;
                            edges.Add(edge);

                            newCube.Vertex[2] = edge.Point1;
                            newCube.Vertex[3] = edge.Point2;

                            //3
                            edge = MergeEdge(prevXCube, 1, prevYCube, 7);
                            edge = edge == null ? new GridLine(new Arguments(x, y, nextZValue)/*3*/, new Arguments(x, y, z)/*0*/) : edge;
                            edges.Add(edge);

                            //4
                            edge = MergeEdge(prevYCube, 0, prevZCube, 6);
                            edge = edge == null ? new GridLine(new Arguments(x, nextYValue, z)/*4*/, new Arguments(nextXValue, nextYValue, z)/*5*/ ) : edge;
                            edges.Add(edge);

                            newCube.Vertex[4] = edge.Point1;
                            newCube.Vertex[5] = edge.Point2;

                            //5
                            edge = new GridLine(new Arguments(nextXValue, nextYValue, z)/*5*/, new Arguments(nextXValue, nextYValue, nextZValue)/*6*/);
                            edges.Add(edge);

                            //6
                            edge = new GridLine(new Arguments(x, nextYValue, nextZValue)/*7*/, new Arguments(nextXValue, nextYValue, nextZValue)/*6*/);
                            edges.Add(edge);

                            newCube.Vertex[6] = edge.Point2;
                            newCube.Vertex[7] = edge.Point1;

                            //7
                            edge = MergeEdge(prevXCube, 5, prevYCube, 3);
                            edge = edge == null ? new GridLine(new Arguments(x, nextYValue, z)/*4*/, new Arguments(x, nextYValue, nextZValue)/*7*/) : edge;
                            edges.Add(edge);

                            //8
                            edge = MergeEdge(prevXCube, 9, prevZCube, 11);
                            edge = edge == null ? new GridLine(new Arguments(x, y, z)/*0*/, new Arguments(x, nextYValue, z)/*4*/) : edge;
                            edges.Add(edge);

                            //9
                            edge = MergeEdge(prevZCube, 10, null, 0);
                            edge = edge == null ? new GridLine(new Arguments(nextXValue, y, z)/*1*/, new Arguments(nextXValue, nextYValue, z)/*5*/ ) : edge;
                            edges.Add(edge);

                            //10
                            edge = new GridLine(new Arguments(nextXValue, y, nextZValue)/*2*/, new Arguments(nextXValue, nextYValue, nextZValue)/*6*/);
                            edges.Add(edge);

                            //11
                            edge = MergeEdge(prevXCube, 10, null, 0);
                            edge = edge == null ? new GridLine(new Arguments(x, y, nextZValue)/*3*/, new Arguments(x, nextYValue, nextZValue)/*7*/) : edge;
                            edges.Add(edge);


                            newCube.Edges = edges.ToArray();

                            toReturn.Add(newCube);

                        }

                        prevX = x;
                        row++;
                        x = nextXValue;
                    }

                    totalRows = currentRow;

                    prevDepth = depth;
                    depth++;
                    z = nextZValue;
                }

                prevColumn = column;
                column++;
                y = nextYValue;
            }

            return toReturn;
        }

        private static GridLine MergeEdge(GridCube cube1,
            int index1,
            GridCube cube2,
            int index2)
        {
            if (cube1 != null)
            {
                return cube1.Edges[index1];
            }

            if (cube2 != null)
            {
                return cube2.Edges[index2];
            }

            return null;
        }

        private static GridRectangle CreateCubeSide(GridRectangleSide side, GridLine freeLine1, GridLine freeLine2, GridRectangle first, GridRectangle second)
        {
            int index = (int)side;
            return new GridRectangle(first.Lines[index], freeLine1, second.Lines[index], freeLine2);
        }

        private static GridRectangle CreateCubeSide(GridLine rightLine, GridLine topLine, GridLine leftLine, GridLine bottomLine)
        {
            return new GridRectangle(rightLine, topLine, leftLine, bottomLine);
        }

        public GridRectangle[][] CloneRectanglesWithChangedZ(GridRectangle[][] rectangles, double z)
        {
            GridRectangle[][] rectanglesToReturn = new GridRectangle[rectangles.Length][];
            for (int column = 0; column < rectangles.Length; column++)
            {
                rectanglesToReturn[column] = new GridRectangle[rectangles[column].Length];
                for (int row = 0; row < rectangles[column].Length; row++)
                {
                    GridLine bottom = null;

                    if (column == 0)
                        bottom = rectangles[column][row].BottomLine.CloneLine();
                    else
                        bottom = rectanglesToReturn[column - 1][row].TopLine;

                    GridLine left = null;
                    if (row == 0)
                        left = rectangles[column][row].LeftLine.CloneLine();
                    else
                        left = rectanglesToReturn[column][row - 1].RightLine;
                    GridLine right = rectangles[column][row].RightLine.CloneLine();
                    GridLine top = rectangles[column][row].TopLine.CloneLine();

                    rectanglesToReturn[column][row] = new GridRectangle(right, top, left, bottom);
                    rectanglesToReturn[column][row].SetZ(z);
                }
            }
            return rectanglesToReturn;
        }

        /// <summary>
        /// Build Mesh in 2D with constant z.
        /// </summary>
        public GridRectangle[][] BuildGridLines(Region3D region, double z, double meshSize)
        {
            GridRectangle[][] rectangles = null;
            var width = region.MinX + region.Width;
            var height = region.MinY + region.Height;
            int rows = 0;
            int columns = 0;
            try
            {
                rows = Convert.ToInt32(Math.Ceiling(width / meshSize));
                columns = Convert.ToInt32(Math.Ceiling(height / meshSize));

            }
            catch (OverflowException)
            {
                //TODO handle this stuff!
                rectangles = new GridRectangle[0][];
                return rectangles;
            }
            Arguments[][] dots = new Arguments[columns + 1][];

            for (int column = 0; column <= columns; column++)
            {
                var currentRow1 = dots[column] = new Arguments[rows + 1];

                var y = region.MinY;
                y = y + column * meshSize;
                if (column == columns)
                    y = height;

                for (int row = 0; row <= rows; row++)
                {
                    //Generate X Y Z

                    var x = region.MinX;
                    x = x + row * meshSize;

                    if (row == rows)
                        x = width;

                    currentRow1[row] = new Arguments(x, y, z);
                }
            }

            if (rectangles == null)
                rectangles = new GridRectangle[columns][];

            //start rectangles creation
            for (int column = 0; column < columns; column++)
            {
                rectangles[column] = new GridRectangle[rows];
                for (int row = 0; row < rows; row++)
                {
                    GridLine bottom = null;

                    if (column == 0)
                        bottom = new GridLine(dots[column][row], dots[column][row + 1]);
                    else
                        bottom = rectangles[column - 1][row].TopLine;

                    GridLine left = null;
                    if (row == 0)
                        left = new GridLine(dots[column][row], dots[column + 1][row]);
                    else
                        left = rectangles[column][row - 1].RightLine;

                    GridLine right = new GridLine(dots[column][row + 1], dots[column + 1][row + 1]);
                    GridLine top = new GridLine(dots[column + 1][row], dots[column + 1][row + 1]);

                    rectangles[column][row] = new GridRectangle(right, top, left, bottom);
                }
            }

            this.rectangles = rectangles;
            return rectangles;
        }



        private void AnalyzeCubes(List<GridCube> cubes, double value, GoldenSectionSearch algoritm, bool isAdaptive)
        {
            foreach (var cube in cubes)
            {
                ///AnalyzeRectangles(cube, value, algoritm, isAdaptive);
            }
        }

        //private void AnalyzeRectangles(GridRectangle[] rectangles, double value, GoldenSectionSearch algoritm, bool isAdaptive)
        //{
        //    foreach (var rec in rectangles)
        //    {
        //        foreach (var line in rec.Lines)
        //        {
        //            if (!line.IsAnalyzed)
        //            {
        //                line.AxisPoint1 = ToAxisPoint(line.Point1);
        //                line.AxisPoint2 = ToAxisPoint(line.Point2);
        //                line.CalculatedValue1 = function.Calculate(line.AxisPoint1);
        //                line.CalculatedValue2 = function.Calculate(line.AxisPoint2);
        //                line.IsAnalyzed = true;
        //                if (line.CalculatedValue1 < value != line.CalculatedValue2 < value)//Через линию проходит линия уровня
        //                {
        //                    line.IsContainsCountorLine = true;
        //                    if (!line.FoundMinium)
        //                    {
        //                        if (isAdaptive)
        //                        {
        //                            line.MiniumF = line.Minium = GetLineMinium(algoritm, line, value);
        //                        }
        //                        else
        //                        {
        //                            line.MiniumF = line.Minium = line.GetCenterPoint();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private Arguments ToAxisPoint(Arguments args)
        {
            return args;
        }

        //private void AnalyzeAndInterpolate(GridLine line, double value)
        //{
        //            if (!line.IsAnalyzed)
        //            {
        //                line.AxisPoint1 = scaler.ToAxisPoint(line.Point1);
        //                line.AxisPoint2 = scaler.ToAxisPoint(line.Point2);
        //                line.CalculatedValue1 = function.Calculate(line.AxisPoint1);
        //                line.CalculatedValue2 = function.Calculate(line.AxisPoint2);
        //                line.IsAnalyzed = true;
        //                line.FoundMinium = false;
        //                if (line.CalculatedValue1 <= value != line.CalculatedValue2 <= value)//Через линию проходит линия уровня
        //                {
        //                    line.IsContainsCountorLine = true;
        //                    line.Minium=line.GetCenterPoint();
        //                }
        //            }
        //}
   
        public MarchingResultsContainer CreateContourLine(Arguments point, bool isAdaptive)
        {
            return CreateContourLine(function.Calculate(point), isAdaptive);
        }
        public List<GridTriangle> triangles;
        private void AnalyzeCubes(List<GridCube> cubes, List<Variable> countroLines, GoldenSectionSearch algoritm, bool isAdaptive)
        {
            foreach (var cube in cubes)
            {
                foreach (var line in cube.Edges)
                {
                    if (!line.IsAnalyzed)
                    {
                        line.AxisPoint1 = ToAxisPoint(line.Point1);
                        line.AxisPoint2 = ToAxisPoint(line.Point2);
                        line.CalculatedValue1 = function.Calculate(line.AxisPoint1);
                        line.CalculatedValue2 = function.Calculate(line.AxisPoint2);
                        line.IsAnalyzed = true;
                        foreach (var countrorLine in countroLines)
                        {
                            if (line.CalculatedValue1 < countrorLine.Value != line.CalculatedValue2 < countrorLine.Value)//Через линию проходит линия уровня
                            {
                                var current = line.CountorData.GetItem(countrorLine.Value);
                                if (current == null || !current.IsFoundMinium)
                                {
                                    current = new GridLineCountorData()
                                    {
                                        IsContainsCountorLine = true,
                                        Value = countrorLine.Value
                                    };
                                    if (isAdaptive)
                                        line.MiniumF = line.Minium = GetLineMinium(algoritm, line, countrorLine.Value);
                                    else
                                        line.MiniumF = line.Minium = line.GetCenterPoint();
                                    current.Minium = current.MiniumF = line.MiniumF;
                                    line.CountorData.Add(current);
                                }
                            }
                        }
                    }
                }
            }
        }

        public List<MarchingResultsContainer> AnalyzeAndCreate(List<Variable> countorLines, bool isAdaptive)
        {
            GoldenSectionSearch goldenRatioAlgoritm = null;
            if (isAdaptive)
            {
                goldenRatioAlgoritm = new GoldenSectionSearch(this.function);
                goldenRatioAlgoritm.AccuracyEpsilon = 0.005;
            }
            AnalyzeCubes(cubes, countorLines, goldenRatioAlgoritm, isAdaptive);
            triangles = new List<GridTriangle>();
            List<MarchingResultsContainer> toReturn = new List<MarchingResultsContainer>();
            foreach (var countorLine in countorLines)
            {
                var current = new MarchingResultsContainer() { Value = countorLine.Value };
                foreach (var cube in cubes)
                {
                    current.Triangles.AddRange(GetTriangles(cube, countorLine.Value));
                }
                toReturn.Add(current);
            }
            return toReturn;
        }

        private List<GridTriangle> GetTriangles(GridCube cube, double value)
        {
            //var edjes = cube.GetEdges();
            //var index = cube.GetCubeIndex(value);
            List<GridTriangle> toReturn = new List<GridTriangle>();
            //for (int i = 0; EdgeTable.Table[index, i] != -1; i += 3)
            //{
            //    var triangle = new GridTriangle()
            //    {
            //        Point1 = edjes[EdgeTable.Table[index, i]].CountorData.GetItem(value).Minium,
            //        Point2 = edjes[EdgeTable.Table[index, i + 1]].CountorData.GetItem(value).Minium,
            //        Point3 = edjes[EdgeTable.Table[index, i + 2]].CountorData.GetItem(value).Minium
            //    };
            //    toReturn.Add(triangle);
            //    if (true)//if smooth faced
            //    {
            //        if (!triangle.IsInvalid)
            //        {
            //            //  var normal = VectorHelper.CalcNormal(triangle);
            //            //  VectorHelper.AddNormals(triangle, normal);
            //        }
            //    }
            //}
            return toReturn;
        }

        public MarchingResultsContainer CreateContourLine(double value, bool isAdaptive)
        {
            GoldenSectionSearch goldenRatioAlgoritm = null;
            if (isAdaptive)
            {
                goldenRatioAlgoritm = new GoldenSectionSearch(this.function);
                goldenRatioAlgoritm.AccuracyEpsilon = 0.005;
            }
            AnalyzeCubes(cubes, value, goldenRatioAlgoritm, isAdaptive);
            MarchingResultsContainer results = new MarchingResultsContainer() { Value = value };
            foreach (var cube in cubes)
            {
                results.Triangles.AddRange(GetTriangles(cube, value));
            }

            return results;
        }

        public int GetIndexToOptimize(Line line)
        {
            int index = 0;
            foreach (var var in line.Point1)
            {
                if (line.Point2[index].Value != line.Point1[index].Value)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public Arguments GetLineMinium(IOptimizationAlgoritm algoritm, Line line, double minium)
        {
            return algoritm.GetMinium(new Arguments(line.AxisPoint1), new Arguments(line.AxisPoint2), new Arguments(line.AxisPoint1), GetIndexToOptimize(line), true, minium);
        }
    }

    public class MarchingResultsContainer
    {
        public MarchingResultsContainer()
        {
            Triangles = new List<GridTriangle>();
        }
        public double Value { get; set; }
        public List<GridTriangle> Triangles { get; set; }

        public List<Line> GetWiredResults()
        {
            var wires = new List<Line>();
            foreach (var triangle in Triangles)
            {
                if (triangle.IsInvalid)
                    continue;
                wires.Add(new Line(triangle.Point1, triangle.Point2));
                wires.Add(new Line(triangle.Point2, triangle.Point3));
                wires.Add(new Line(triangle.Point3, triangle.Point1));
            }
            return wires;
        }
    }

}



