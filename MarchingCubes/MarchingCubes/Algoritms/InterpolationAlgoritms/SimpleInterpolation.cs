using System;
using System.Collections.Generic;
using System.Text;
using MarchingCubes.CommonTypes;

namespace MarchingCubes.Algoritms.InterpolationAlgoritms
{
    /// <summary>
    /// Predict isosurface without any additinal function calculation.
    /// </summary>
    public class SimpleInterpolation : IinterpolationAlgoritm
    {
        private const double e = 0.00001;
        bool isCenter = false;
        public SimpleInterpolation(bool isSetCenter = false)
        {
            this.isCenter = isSetCenter;
        }

        public Point GetCenterPoint(Point point, Point point2)
        {
            return new Point((point.X + point2.X) / 2, (point.Y + point2.Y) / 2, (point.Z + point2.Z) / 2);
        }

        public Point Interpolate(Point start, Point finish, int axissIndex, double? isolevel = null)
        {
            if (isCenter)
            {
                return GetCenterPoint(start, finish);
            }

            if (isolevel == null)
            {
                throw new ArgumentNullException(nameof(isolevel));
            }

            var iso = isolevel.Value;
            var startValue = start.Get(axissIndex) > finish.Get(axissIndex) ? finish.Get(axissIndex) : start.Get(axissIndex);
            var finishValue = start.Get(axissIndex) > finish.Get(axissIndex) ? start.Get(axissIndex) : finish.Get(axissIndex);

            //TODO: calc
            var calcA = start.CalculatedValue.Value;
            var calcB = finish.CalculatedValue.Value;
            if (Math.Abs(iso - calcA) < e)
                return start;
            if (Math.Abs(iso - calcB) < e)
                return finish;
            if (Math.Abs(calcB - calcA) < e)
                return finish;
            var suggestion = (iso - calcA) / (calcB - calcA);

            var x = start.X + suggestion * (finish.X - start.X);
            var y = start.Y + suggestion * (finish.Y - start.Y);
            var z = start.Z + suggestion * (finish.Z - start.Z);
            var result = new Point(x, y, z);

            return result;
        }
    }
}
