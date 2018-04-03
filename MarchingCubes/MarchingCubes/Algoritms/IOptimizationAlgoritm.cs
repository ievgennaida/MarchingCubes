using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarchingCubes.CommonTypes;

namespace MarchingCubes.Algoritms
{
    /// <summary>
    /// Одномірна оптимізація
    /// </summary>
    public interface IOptimizationAlgoritm
    {
        Arguments GetMinium();
        Arguments GetMinium(double start, double finish);
        Arguments GetMinium(Arguments start, Arguments finish, Arguments point, int index, bool useMinium, double minium);
    }
}
