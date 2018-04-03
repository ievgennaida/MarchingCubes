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
        public void Test()
        {
            var marchingCubes = new AdaptiveMarchingCubes(null);
            var region = new CommonTypes.Region3D(0, 10, 0, 10, 0, 10);
            var cubes = marchingCubes.BuildGrid(region, 5);
            
            Assert.AreEqual(cubes.Count, 8);

            // 18 unique edges
            var  lines = new List<GridLine>();
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

            Assert.AreEqual(lines.Count, 18);

    
        }
    }
}
