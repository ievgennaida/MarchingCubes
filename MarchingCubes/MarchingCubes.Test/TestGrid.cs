using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarchingCubes.Algoritms.CountorLines;
using System.Collections.Generic;
using System.Linq;

namespace MarchingCubes.Test
{
    [TestClass]
    public class TestGrid
    {
        [TestMethod]
        public void CheckOneCube()
        {
            var size = 10;
            var gridSize = 10;
            Сheck(size, gridSize);
        }

        [TestMethod]
        public void Check8Cubes()
        {
            var size = 10;
            var gridSize = 5;
            Сheck(size, gridSize);
        }

        [TestMethod]
        public void Check27Cubes()
        {
            var size = 15;
            var gridSize = 5;
            Сheck(size, gridSize);
        }

        public List<GridCube> Сheck(int size, int step)
        {
            var edgesPerSide = size / step;
            var countOfCubes = Math.Pow(edgesPerSide, 3);

            var vertexPerSide = edgesPerSide + 1;
            var uniqueEdgesPerSide = edgesPerSide * vertexPerSide;
            uniqueEdgesPerSide += uniqueEdgesPerSide;

            var edjesCount = vertexPerSide * uniqueEdgesPerSide + (vertexPerSide * vertexPerSide * edgesPerSide);


            var marchingCubes = new Algoritms.CountorLines.MarchingCubes(null);
            var region = new CommonTypes.Region3D(0, size, 0, size- step- step, 0, size);
            var cubes = marchingCubes.BuildGrid(region, step);

            Assert.AreEqual(cubes.Count, countOfCubes);

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
            }

            Assert.AreEqual(lines.Count, edjesCount);

            // Check last cube vertex
            var firstCube = cubes.FirstOrDefault();
            CheckVertexes(firstCube, 0, step);
            var lastCube = cubes.LastOrDefault();
            CheckVertexes(lastCube, size - step, size);
            return cubes;
        }

        private static void CheckVertexes(GridCube cube, int start, int end)
        {
            Assert.AreEqual(cube.Vertex[0], new CommonTypes.Arguments(start, start, start));
            Assert.AreEqual(cube.Vertex[1], new CommonTypes.Arguments(end, start, start));
            Assert.AreEqual(cube.Vertex[2], new CommonTypes.Arguments(end, start, end));
            Assert.AreEqual(cube.Vertex[3], new CommonTypes.Arguments(start, start, end));

            Assert.AreEqual(cube.Vertex[4], new CommonTypes.Arguments(start, end, start));
            Assert.AreEqual(cube.Vertex[5], new CommonTypes.Arguments(end, end, start));
            Assert.AreEqual(cube.Vertex[6], new CommonTypes.Arguments(end, end, end));
            Assert.AreEqual(cube.Vertex[7], new CommonTypes.Arguments(start, end, end));
        }
    }
}
