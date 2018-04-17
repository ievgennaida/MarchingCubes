using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarchingCubes.Algoritms.CountorLines;
using System.Collections.Generic;

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
            
            var one = Сheck(size, gridSize);
            var cube = one[0];
            Assert.AreEqual(cube.Vertex[0], new CommonTypes.Arguments(0, 0, 0));
            Assert.AreEqual(cube.Vertex[1], new CommonTypes.Arguments(gridSize, 0, 0));
            Assert.AreEqual(cube.Vertex[2], new CommonTypes.Arguments(gridSize, 0, gridSize));
            Assert.AreEqual(cube.Vertex[3], new CommonTypes.Arguments(0, 0, gridSize));

            Assert.AreEqual(cube.Vertex[4], new CommonTypes.Arguments(0, gridSize, 0));
            Assert.AreEqual(cube.Vertex[5], new CommonTypes.Arguments(gridSize, gridSize, 0));
            Assert.AreEqual(cube.Vertex[6], new CommonTypes.Arguments(gridSize, gridSize, gridSize));
            Assert.AreEqual(cube.Vertex[7], new CommonTypes.Arguments(0, gridSize, gridSize));
        }

        [TestMethod]
        public void CheckCountOfUniqueLines()
        {
            var size = 10;
            var gridSize = 5;

            // side = 12
            // sides count 3
            // margins for side =9
            // count of margins = 2

            //uniqueEdgesCount = oneSideEdjes * (vertexPerSide + 1) + ((vertexPerSide + 1) * (vertexPerSide + 1));


            Сheck(size, gridSize);
        }

        public List<GridCube> Сheck(int size, int gridSize)
        {
            var edgesPerSide = size / gridSize;
            var countOfCubes = Math.Pow(edgesPerSide,3);

            var vertexPerSide = edgesPerSide + 1;
            var uniqueEdgesPerSide = edgesPerSide * vertexPerSide;
            uniqueEdgesPerSide += uniqueEdgesPerSide;

            var edjesCount = vertexPerSide * uniqueEdgesPerSide + (vertexPerSide * vertexPerSide * edgesPerSide);


            var marchingCubes = new AdaptiveMarchingCubes(null);
            var region = new CommonTypes.Region3D(0, size, 0, size, 0, size);
            var cubes = marchingCubes.BuildGrid(region, gridSize);

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

            return cubes;
        }
    }
}
