using System;
using System.Collections.Generic;
using System.Text;
using MarchingCubes.CommonTypes;

namespace MarchingCubes.Algoritms.InterpolationAlgoritms
{
    /// <summary>
    /// Predict isosurface without any additinal function calculation.
    /// </summary>
    public class SimpleInterpolation : INterpolationAlgoritm
    {
        private const double e = 0.00001;
        public SimpleInterpolation()
        {

        }

        public Arguments Interpolate(Arguments start, Arguments finish, int axissIndex, double? isolevel = null)
        {
            if (isolevel == null)
            {
                throw new ArgumentNullException(nameof(isolevel));
            }

            var iso = isolevel.Value;
            var startValue = start[axissIndex].Value > finish[axissIndex].Value ? finish[axissIndex].Value : start[axissIndex].Value;
            var finishValue = start[axissIndex].Value > finish[axissIndex].Value ? start[axissIndex].Value : finish[axissIndex].Value;

            //TODO: calc
            var calcA = double.MaxValue;
            var calcB = double.MaxValue;
            if (Math.Abs(iso - calcA) < e)
                return start;
            if (Math.Abs(iso - calcB) < e)
                return finish;
            if (Math.Abs(calcB - calcA) < e)
                return finish;
            var suggestion = (isolevel - calcA) / (calcB - calcA);

            var result = new Arguments(
                start.X + suggestion * (finish.X - start.X),
                start.Y + suggestion * (finish.Y - start.Y),
                start.Z + suggestion * (finish.Z - start.Z));

            return result;
        }
    }
}
