using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GradientDescent.CommonTypes;

namespace GradientDescentLogic.Algoritms.CountorLines
{
    public class GridLineCountorData
    {
        /// <summary>
        /// for real value
        /// </summary>
        public Arguments Minium { get; set; }

        /// <summary>
        /// for screen point
        /// </summary>
        public Arguments MiniumF { get; set; }

        public bool IsFoundMinium
        {
            get
            {
                return Minium != null && MiniumF != null;
            }
        }

        /// <summary>
        /// Countor Line Value
        /// </summary>
        public double Value { get; set; }

        public bool IsContainsCountorLine { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != this.GetType())
                return false;
            var objc = (GridLineCountorData)obj;

            if (Object.ReferenceEquals(obj, this))
                return true;

            return objc.Value == this.Value;
        }
        public static bool operator ==(GridLineCountorData r1, GridLineCountorData r2)
        {
            if (Object.ReferenceEquals(r1, null))
            {
                return Object.ReferenceEquals(r2, null);
            }
            else 
            {
                return r1.Equals(r2);
            }

        }
        public static bool operator !=(GridLineCountorData r1, GridLineCountorData r2)
        {
            return !(r1 == r2);
        }
    }

    public class GridCountorsList : List<GridLineCountorData>
    {
        public bool IsContains(double value)
        {
            return GetItem(value) != null;
        }

        public GridLineCountorData GetItem(double value)
        {
            foreach (var item in this)
            {
                if (item.Value == value)
                    return item;
            }
            return null;
        }
    }
}
