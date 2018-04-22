using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarchingCubes.CommonTypes;

namespace MarchingCubes.Algoritms
{
    public interface INterpolationAlgoritm
    {
        /// <summary>
        /// Find nearest isolane.
        /// </summary>
        /// <param name="from">Search from position.</param>
        /// <param name="to">Search to position.</param>
        /// <param name="axissIndex">Axiss to move on. Algoritm can search only in one dimension.</param>
        /// <param name="isolevel">Find min value if null</param>
        /// <returns>The nearest point.</returns>
        Arguments Interpolate(Arguments from, Arguments to, int axissIndex, double? isolevel = null);
    }
}
