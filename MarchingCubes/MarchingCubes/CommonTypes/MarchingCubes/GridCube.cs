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
    }
}


