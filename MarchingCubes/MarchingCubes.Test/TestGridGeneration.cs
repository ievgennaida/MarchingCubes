using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarchingCubes.Algoritms.CountorLines;
using System.Collections.Generic;
using System.Linq;
using MarchingCubes.Algoritms.MarchingCubes;
using MarchingCubes.CommonTypes;

namespace MarchingCubes.Test
{
    [TestClass]
    public class TestGridGeneration
    {
        [TestMethod]
        public void CheckOneCube()
        {
            var size = 10f;
            var gridSize = 10f;
            Сheck(0f, size, gridSize);
        }
        //  Step = 0.3,
        //        Region = new Region3D(-1.5, 1.5, -1.5, 1.5, -1.5, 1.5),

        [TestMethod]
        public void Check8Cubes()
        {
            var size = 10f;
            var gridSize = 5f;
            Сheck(0f, size, gridSize);
        }

        [TestMethod]
        public void CheckCubes()
        {
            var gridSize = 0.5f;
            Сheck(-1.5, 1.5, gridSize);
        }


        [TestMethod]
        public void CheckCubesFloat()
        {
            var gridSize = 0.5f;
            Сheck(-0.5, 0, gridSize);
        }

        [TestMethod]
        public void Check27Cubes()
        {
            var size = 15f;
            var gridSize = 5f;
            Сheck(0, size, gridSize);
        }

        public List<GridCube> Сheck(double from, double to, double step)
        {
            var edgesPerSide = Region3D.GetLenght(from, to) / step;
            var countOfCubes = Math.Pow(edgesPerSide, 3);

            var vertexPerSide = edgesPerSide + 1;
            var uniqueEdgesPerSide = edgesPerSide * vertexPerSide;
            uniqueEdgesPerSide += uniqueEdgesPerSide;

            var edjesCount = vertexPerSide * uniqueEdgesPerSide + (vertexPerSide * vertexPerSide * edgesPerSide);


            var marchingCubes = new MarchingCubesAlgorithm(null);
            var region = new CommonTypes.Region3D(from, to, from, to, from, to);
            var cubes = marchingCubes.BuildGrid(region, step);

            Assert.AreEqual(cubes.Count, countOfCubes);

            var uniqueVertexes = new List<Point>();
            // 18 unique edges
            var lines = new List<GridLine>();
            foreach (var cube in cubes)
            {
                foreach (var edge in cube.Edges)
                {
                    if (!lines.Contains(edge))
                    {
                        lines.Add(edge);
                    }
                }

                foreach (var vert in cube.Vertex)
                {
                    // Check that objects are not created. Exclude overriden values.
                    //if (!uniqueVertexes.Any(p => object.ReferenceEquals(uniqueVertexes, vert)))
                    if(!uniqueVertexes.Contains(vert))
                    {
                        uniqueVertexes.Add(vert);
                    }
                }
            }

            var totalVertexes = vertexPerSide * vertexPerSide * vertexPerSide;
            Assert.AreEqual(uniqueVertexes.Count, totalVertexes);

            Assert.AreEqual(lines.Count, edjesCount);

            // Check last cube vertex
            var firstCube = cubes.FirstOrDefault();
            CheckVertexes(firstCube, from, from+step);
            var lastCube = cubes.LastOrDefault();
            CheckVertexes(lastCube, to - step, to);
            return cubes;
        }

        private static void CheckVertexes(GridCube cube, double start, double end)
        {
            Assert.AreEqual(cube.Vertex[0], new CommonTypes.Point(start, start, start));
            Assert.AreEqual(cube.Vertex[1], new CommonTypes.Point(end, start, start));
            Assert.AreEqual(cube.Vertex[2], new CommonTypes.Point(end, start, end));
            Assert.AreEqual(cube.Vertex[3], new CommonTypes.Point(start, start, end));

            Assert.AreEqual(cube.Vertex[4], new CommonTypes.Point(start, end, start));
            Assert.AreEqual(cube.Vertex[5], new CommonTypes.Point(end, end, start));
            Assert.AreEqual(cube.Vertex[6], new CommonTypes.Point(end, end, end));
            Assert.AreEqual(cube.Vertex[7], new CommonTypes.Point(start, end, end));
        }
    }
}
