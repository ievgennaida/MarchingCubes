using MarchingCubes.Algoritms.CountorLines;
using MarchingCubes.Algoritms.InterpolationAlgoritms;
using MarchingCubes.CommonTypes;
using System;
using System.Collections.Generic;

namespace MarchingCubes.Algoritms.MarchingCubes
{
    /// <summary>
    /// Marching cubes algorithm.
    /// </summary>
    public class MarchingCubesAlgorithm
    {
        protected MarchingFunction function;
        protected IinterpolationAlgoritm Interpolation { get; set; }
        public MarchingCubesAlgorithm(MarchingFunction function, IinterpolationAlgoritm interpolation = null)
        {
            this.function = function;
            this.Interpolation = interpolation;
        }

        /// <summary>
        /// Create a grid to build a model based on.
        /// </summary>
        /// <returns>List of the created cubes.</returns>
        public List<GridCube> BuildGrid(Region3D region, double step)
        {
            if (step <= 0)
            {
                throw new ArgumentException(nameof(step) + " cannot be <=0");
            }

            var toReturn = new List<GridCube>();
            var totalX = 0;
            var totalZ = 0;

            var column = 0;
            var prevColumn = 0;
            for (var y = region.MinY; y < region.MaxY;)
            {
                var isLastColumn = (y + step) > region.MaxY;
                var isFirstY = column == 0;
                var nextYValue = isLastColumn ? region.MaxY : y + step;

                var depth = 0;
                var prevDepth = 0;
                var zIndex = 0;
                for (var z = region.MinZ; z < region.MaxZ;)
                {
                    var isLastDepth = (z + step) > region.MaxZ;
                    var isFirstZ = depth == 0;
                    var nextZValue = isLastDepth ? region.MaxZ : z + step;
                    zIndex++;

                    var xIndex = 0;
                    var prevX = region.MinX;
                    for (var x = region.MinX; x < region.MaxX;)
                    {
                        var isLastX = (x + step) > region.MaxX;
                        var isFirstX = xIndex == 0;
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
                                var index = toReturn.Count - totalZ * totalX;
                                prevYCube = toReturn[index];
                            }

                            GridCube prevZCube = null;
                            if (!isFirstZ)
                            {
                                var index = toReturn.Count - totalX;
                                prevZCube = toReturn[index];
                            }

                            var newCube = new GridCube();

                            // Dont create object duplicates. Cubes edges are merged. 
                            var edges = new List<GridLine>();
                            //0
                            var edge = GetMergeEdge(prevYCube, 4, prevZCube, 2);
                            edge = edge == null ? new GridLine(new Point(x, y, z)/*0*/, new Point(nextXValue, y, z)/*1*/, AxissConsts.X) : edge;
                            edges.Add(edge);
                            newCube.Vertex[0] = edge.Point1;
                            newCube.Vertex[1] = edge.Point2;

                            //1
                            edge = GetMergeEdge(prevYCube, 5);//, prevXCube, 3);
                            edge = edge ?? new GridLine(newCube.Vertex[1]/*1*/, new Point(nextXValue, y, nextZValue)/*2*/, AxissConsts.Z);
                            edges.Add(edge);

                            newCube.Vertex[2] = edge.Point2;

                            //2
                            edge = GetMergeEdge(prevYCube, 6);//, prevZCube, 0);
                            edge = edge ?? new GridLine(new Point(x, y, nextZValue)/*3*/, newCube.Vertex[2]/*2*/, AxissConsts.X);
                            edges.Add(edge);

                            newCube.Vertex[3] = edge.Point1;

                            //3
                            edge = GetMergeEdge(prevXCube, 1, prevYCube, 7);
                            edge = edge ?? new GridLine(newCube.Vertex[3]/*3*/, newCube.Vertex[0]/*0*/, AxissConsts.Z);
                            edges.Add(edge);

                            //4
                            edge = GetMergeEdge(prevZCube, 6);
                            edge = edge ?? new GridLine(new Point(x, nextYValue, z)/*4*/, new Point(nextXValue, nextYValue, z)/*5*/ , AxissConsts.X);
                            edges.Add(edge);

                            newCube.Vertex[4] = edge.Point1;
                            newCube.Vertex[5] = edge.Point2;

                            //5
                            edge = new GridLine(newCube.Vertex[5]/*5*/, new Point(nextXValue, nextYValue, nextZValue)/*6*/, AxissConsts.Z);
                            edges.Add(edge);
                            newCube.Vertex[6] = edge.Point2;

                            //6
                            edge = new GridLine(new Point(x, nextYValue, nextZValue)/*7*/, newCube.Vertex[6]/*6*/, AxissConsts.X);
                            edges.Add(edge);

                    
                            newCube.Vertex[7] = edge.Point1;

                            //7 
                            edge = GetMergeEdge(prevXCube, 5);
                            edge = edge ?? new GridLine(newCube.Vertex[4]/*4*/, newCube.Vertex[7]/*7*/, AxissConsts.Y);
                            edges.Add(edge);

                            //8
                            edge = GetMergeEdge(prevXCube, 9, prevZCube, 11);
                            edge = edge == null ? new GridLine(newCube.Vertex[0]/*0*/, newCube.Vertex[4]/*4*/, AxissConsts.Y) : edge;
                            edges.Add(edge);

                            //9
                            edge = GetMergeEdge(prevZCube, 10);
                            edge = edge ?? new GridLine(newCube.Vertex[1]/*1*/, newCube.Vertex[5]/*5*/, AxissConsts.Y);
                            edges.Add(edge);

                            //10
                            edge = new GridLine(newCube.Vertex[2]/*2*/, newCube.Vertex[6]/*6*/, AxissConsts.Y);
                            edges.Add(edge);

                            //11
                            edge = GetMergeEdge(prevXCube, 10, null, 0);
                            edge = edge ?? new GridLine(newCube.Vertex[3]/*3*/, newCube.Vertex[7]/*7*/, AxissConsts.Y);
                            edges.Add(edge);


                            newCube.Edges = edges.ToArray();

                            toReturn.Add(newCube);
                        }

                        prevX = x;
                        xIndex++;
                        x = nextXValue;
                    }

                    totalX = xIndex;
                    prevDepth = depth;
                    depth++;
                    z = nextZValue;
                }

                totalZ = zIndex;
                prevColumn = column;
                column++;
                y = nextYValue;
            }

            return toReturn;
        }

        protected GridLine GetMergeEdge(GridCube cube1,
            int index1,
            GridCube cube2 = null,
            int index2 = 0)
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

        private List<Triangle> GetTriangles(GridCube cube, double value)
        {
            var edjes = cube.Edges;
            var index = GetCubeIndex(cube, value);
            var toReturn = new List<Triangle>();

            for (int i = 0; EdgesTable.Table[index, i] != -1; i += 3)
            {
                var vertex1 = edjes[EdgesTable.Table[index, i]];
                var vertex2 = edjes[EdgesTable.Table[index, i + 1]];
                var vertex3 = edjes[EdgesTable.Table[index, i + 2]];

                var triangle = new Triangle(vertex1.IsoPoint, vertex2.IsoPoint, vertex3.IsoPoint);
                var normalVector = triangle.GetNormal();
                // All triangle points are merged togeher (Triangles share vertexes). 
                // This is used to show smooth model.
                triangle.Point1.AddAndMergeNormal(normalVector);
                triangle.Point2.AddAndMergeNormal(normalVector);
                triangle.Point3.AddAndMergeNormal(normalVector);
                toReturn.Add(triangle);
            }

            return toReturn;
        }

        /// <summary>
        /// Get special index of cube isolevel for marching cubes algoritm
        /// </summary>
        /// <param name="cube"></param>
        /// <param name="isolevel"></param>
        /// <returns></returns>
        public int GetCubeIndex(GridCube cube, double isolevel)
        {
            var points = cube.Vertex;
            int cubeIndex = 0;
            if (points[0].CalculatedValue.HasValue && points[0].CalculatedValue.Value < isolevel) cubeIndex |= 1;
            if (points[1].CalculatedValue.HasValue && points[1].CalculatedValue.Value < isolevel) cubeIndex |= 2;
            if (points[2].CalculatedValue.HasValue && points[2].CalculatedValue.Value < isolevel) cubeIndex |= 4;
            if (points[3].CalculatedValue.HasValue && points[3].CalculatedValue.Value < isolevel) cubeIndex |= 8;
            if (points[4].CalculatedValue.HasValue && points[4].CalculatedValue.Value < isolevel) cubeIndex |= 16;
            if (points[5].CalculatedValue.HasValue && points[5].CalculatedValue.Value < isolevel) cubeIndex |= 32;
            if (points[6].CalculatedValue.HasValue && points[6].CalculatedValue.Value < isolevel) cubeIndex |= 64;
            if (points[7].CalculatedValue.HasValue && points[7].CalculatedValue.Value < isolevel) cubeIndex |= 128;
            return cubeIndex;
        }

        private double? CalculateOrGetFromCache(MarchingFunction function, Point toCheck)
        {
            try
            {
                var value = function(toCheck);
                return value;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Cannot calculate function in the point:" + toCheck.ToString() + ". " + ex.Message);
            }

            return null;
        }

        public MarchingCubesResults Generate(List<GridCube> cubes, double isoLevel)
        {
            var toReturn = new MarchingCubesResults();
            toReturn.Iso = isoLevel;
            toReturn.Grid = cubes;

            foreach (var cube in cubes)
            {
                foreach (var line in cube.Edges)
                {
                    if (!line.IsAnalyzed)
                    {
                        var calcA = line.Point1.CalculatedValue;
                        if (!calcA.HasValue)
                        {
                            calcA = line.Point1.CalculatedValue = CalculateOrGetFromCache(function, line.Point1);
                        }

                        var calcB = line.Point2.CalculatedValue;
                        if (!calcB.HasValue)
                        {
                            calcB = line.Point2.CalculatedValue = CalculateOrGetFromCache(function, line.Point2);
                        }

                        line.IsAnalyzed = true;

                        // Current edge has iso line.
                        if (calcA.HasValue && calcB.HasValue && (calcA.Value < isoLevel != calcB.Value < isoLevel))
                        {
                            if (line.IsoPoint == null)
                            {
                                line.HasIsoLine = true;
                                var interpolation = Interpolation;
                                if (interpolation == null)
                                {
                                    interpolation = new SimpleInterpolation(true);
                                }

                                line.IsoPoint = interpolation.Interpolate(
                                            line.Point1,
                                           line.Point2,
                                           line.AxissIndex,
                                           isoLevel);
                            }
                        }
                    }
                }

                toReturn.Triangles.AddRange(GetTriangles(cube, isoLevel));
            }

            return toReturn;
        }

        public MarchingCubesResults Generate(Region3D region, double step, double isoLevel)
        {
            var grid = this.BuildGrid(region, step);
            var results = Generate(grid, isoLevel);
            return results;
        }
    }
}



