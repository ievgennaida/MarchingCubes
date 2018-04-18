using MarchingCubes.CommonTypes;
using System;
using System.Collections.Generic;
namespace MarchingCubes.Algoritms.MarchingCubes
{
    public class GridCube
    {
        public GridCube()
        {
            Vertex = new Point[8];
        }

        /// <summary>
        /// Gets the collection of edges.
        /// </summary>
        public GridLine[] Edges { get; internal set; }

        /// <summary>
        /// Gets or sets the vertex. 
        /// </summary>
        public Point[] Vertex { get; internal set; }

        public int LastCubeIndex { get; set; }

        /// <summary>
        /// Get special index of cube isolevel for marching cubes algoritm
        /// </summary>
        /// <returns></returns>
        public int GetCubeIndex(double isolevel)
        {
            //var points = GetVertexValues();
            int cubeIndex = 0;
            //if (points[0] < isolevel) cubeIndex |= 1;
            //if (points[1] < isolevel) cubeIndex |= 2;
            //if (points[2] < isolevel) cubeIndex |= 4;
            //if (points[3] < isolevel) cubeIndex |= 8;
            //if (points[4] < isolevel) cubeIndex |= 16;
            //if (points[5] < isolevel) cubeIndex |= 32;
            //if (points[6] < isolevel) cubeIndex |= 64;
            //if (points[7] < isolevel) cubeIndex |= 128;
            //LastCubeIndex = cubeindex;
            return cubeIndex;
        }
    }

}


