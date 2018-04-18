using MarchingCubes.CommonTypes;
using MarchingCubes.GraphicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarchingCubes.Algoritms.MarchingCubes
{
    /// <summary>
    /// Marching cubes algorithm.
    /// </summary>
    public class MarchingCubesAlgorithm
    {
        protected MarchingFunction function;
        protected INterpolationAlgoritm Interpolation { get; set; }
        public MarchingCubesAlgorithm(MarchingFunction function, INterpolationAlgoritm interpolation = null)
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
                            var edge = GetEdge(prevYCube, 4, prevZCube, 2);
                            edge = edge == null ? new GridLine(new Point(x, y, z)/*0*/, new Point(nextXValue, y, z)/*1*/, AxissConsts.X) : edge;
                            edges.Add(edge);
                            newCube.Vertex[0] = edge.Point1;
                            newCube.Vertex[1] = edge.Point2;

                            //1
                            edge = GetEdge(prevYCube, 5);//, prevXCube, 3);
                            edge = edge ?? new GridLine(new Point(nextXValue, y, z)/*1*/, new Point(nextXValue, y, nextZValue)/*2*/, AxissConsts.Z);
                            edges.Add(edge);

                            //2
                            edge = GetEdge(prevYCube, 6);//, prevZCube, 0);
                            edge = edge ?? new GridLine(new Point(x, y, nextZValue)/*3*/, new Point(nextXValue, y, nextZValue)/*2*/, AxissConsts.X);
                            edges.Add(edge);

                            newCube.Vertex[2] = edge.Point2;
                            newCube.Vertex[3] = edge.Point1;

                            //3
                            edge = GetEdge(prevXCube, 1, prevYCube, 7);
                            edge = edge ?? new GridLine(new Point(x, y, nextZValue)/*3*/, new Point(x, y, z)/*0*/, AxissConsts.Z);
                            edges.Add(edge);

                            //4
                            edge = GetEdge(prevZCube, 6);//prevYCube, 0);//, );
                            edge = edge ?? new GridLine(new Point(x, nextYValue, z)/*4*/, new Point(nextXValue, nextYValue, z)/*5*/ , AxissConsts.X);
                            edges.Add(edge);

                            newCube.Vertex[4] = edge.Point1;
                            newCube.Vertex[5] = edge.Point2;

                            //5
                            edge = new GridLine(new Point(nextXValue, nextYValue, z)/*5*/, new Point(nextXValue, nextYValue, nextZValue)/*6*/, AxissConsts.Z);
                            edges.Add(edge);

                            edge = null;
                            //6
                            edge = new GridLine(new Point(x, nextYValue, nextZValue)/*7*/, new Point(nextXValue, nextYValue, nextZValue)/*6*/, AxissConsts.X);
                            edges.Add(edge);

                            newCube.Vertex[6] = edge.Point2;
                            newCube.Vertex[7] = edge.Point1;

                            //7 
                            edge = GetEdge(prevXCube, 5);//, prevYCube, 3);
                            edge = edge ?? new GridLine(new Point(x, nextYValue, z)/*4*/, new Point(x, nextYValue, nextZValue)/*7*/, AxissConsts.Y);
                            edges.Add(edge);

                            //8
                            edge = GetEdge(prevXCube, 9, prevZCube, 11);
                            edge = edge == null ? new GridLine(new Point(x, y, z)/*0*/, new Point(x, nextYValue, z)/*4*/, AxissConsts.Y) : edge;
                            edges.Add(edge);

                            //9
                            edge = GetEdge(prevZCube, 10);
                            edge = edge ?? new GridLine(new Point(nextXValue, y, z)/*1*/, new Point(nextXValue, nextYValue, z)/*5*/, AxissConsts.Y);
                            edges.Add(edge);

                            //10
                            edge = new GridLine(new Point(nextXValue, y, nextZValue)/*2*/, new Point(nextXValue, nextYValue, nextZValue)/*6*/, AxissConsts.Y);
                            edges.Add(edge);

                            //11
                            edge = GetEdge(prevXCube, 10, null, 0);
                            edge = edge ?? new GridLine(new Point(x, y, nextZValue)/*3*/, new Point(x, nextYValue, nextZValue)/*7*/, AxissConsts.Y);
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

        protected GridLine GetEdge(GridCube cube1,
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

        public List<Triangle> triangles;
        private void AnalyzeCubes(List<GridCube> cubes, double isoLevel)
        {
            foreach (var cube in cubes)
            {
                foreach (var line in cube.Edges)
                {
                    //if (!line.IsAnalyzed)
                    //{
                    //    line.CalcA = function.Calculate(line.Point1);
                    //    line.CalcB = function.Calculate(line.Point2);
                    //    line.IsAnalyzed = true;
                    //    foreach (var countrorLine in countroLines)
                    //    {
                    //        if (line.CalcA < countrorLine.Value != line.CalcB < countrorLine.Value)//Через линию проходит линия уровня
                    //        {
                    //            var current = line.CountorData.GetItem(countrorLine.Value);
                    //            if (current == null || !current.IsFoundMinium)
                    //            {
                    //                current = new GridLineCountorData()
                    //                {
                    //                    HasCountourLine = true,
                    //                    IsoLevel = countrorLine.Value
                    //                };

                    //                line.Minium = GetIsoSurfacePoint(line, countrorLine.Value);

                    //                current.Minium = current.MiniumF = line.Minium;
                    //                line.CountorData.Add(current);
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }


        public List<MarchingCubesResults> Generate(params double[] isoLevels)
        {
            var toReturn = new List<MarchingCubesResults>();
            // Generate results for each level.
            foreach (var level in isoLevels)
            {
                //AnalyzeCubes(cubes, countorLines, goldenRatioAlgoritm, isAdaptive);
            }

           
            //triangles = new List<Triangle>();
        
            //foreach (var countorLine in countorLines)
            //{
            //    var current = new MarchingCubesResults() { Value = countorLine.Value };
            //    foreach (var cube in cubes)
            //    {
            //        current.Triangles.AddRange(GetTriangles(cube, countorLine.Value));
            //    }
            //    toReturn.Add(current);
            //}
            return toReturn;
        }

        private List<Triangle> GetTriangles(GridCube cube, double value)
        {
            //var edjes = cube.GetEdges();
            //var index = cube.GetCubeIndex(value);
            var toReturn = new List<Triangle>();
            //for (int i = 0; EdgeTable.Table[index, i] != -1; i += 3)
            //{
            //    var triangle = new GridTriangle()
            //    {
            //        Point1 = edjes[EdgeTable.Table[index, i]].CountorData.GetItem(value).Minium,
            //        Point2 = edjes[EdgeTable.Table[index, i + 1]].CountorData.GetItem(value).Minium,
            //        Point3 = edjes[EdgeTable.Table[index, i + 2]].CountorData.GetItem(value).Minium
            //    };
            //    toReturn.Add(triangle);
            //if (!triangle.IsInvalid)
                //        {
                //            //  var normal = VectorHelper.CalcNormal(triangle);
                //            //  VectorHelper.AddNormals(triangle, normal);
                //        }
                //}
                return toReturn;
        }

        public MarchingCubesResults GenerateI(double isoLevel, INterpolationAlgoritm interpolation)
        {
            if (interpolation == null)
            {
                interpolation = new GoldenSectionSearch(this.function, 0.005);
            }

            //AnalyzeCubes(cubes, isoLevel, interpolation);
            var results = new MarchingCubesResults() { Value = isoLevel };
            //foreach (var cube in cubes)
            //{
            //    results.Triangles.AddRange(GetTriangles(cube, isoLevel));
            //}

            return results;
        }

        public Arguments GetIsoSurfacePoint(GridLine line, double minium)
        {
            return Interpolation.Interpolate(
                new Arguments(line.Point1),
                new Arguments(line.Point2),
                line.Index,
                minium);
        }
    }
}



