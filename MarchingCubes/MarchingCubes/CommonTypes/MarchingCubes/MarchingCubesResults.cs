using System.Collections.Generic;

namespace MarchingCubes.Algoritms.MarchingCubes
{
    public class MarchingCubesResults
    {
        public MarchingCubesResults()
        {
            this.Triangles = new List<Triangle>();
            this.Grid = new List<GridCube>();
        }

        public List<GridCube> Grid { get; internal set; }
        public double Iso { get; set; }
        public List<Triangle> Triangles { get; internal set; }
    }
}
