using MarchingCubes.CommonTypes;
using System;
using System.Collections.Generic;
namespace MarchingCubes.Algoritms.CountorLines
{
    public class GridCube
    {
        public GridCube()
        {
            Vertex = new Arguments[8];
        }

        public GridLine[] Edges { get; set; }

        /// <summary>
        /// Gets or sets the vertex. 
        /// </summary>
        public Arguments[] Vertex { get; set; }

   
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


    public enum GridEdges
    {
        BackBottom = 0,
        RightBottom = 1,
        FrontBottom = 2,
        LeftBottom = 3,

        BackTop = 4,
        RightTop = 5,
        FrontTop = 6,
        LeftTop = 7,

        BackRight = 8,
        BackLeft = 9,

        FrontLeft = 10,
        FrontRight = 11
    }
}


