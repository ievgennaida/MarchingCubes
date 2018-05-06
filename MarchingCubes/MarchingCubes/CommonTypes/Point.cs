using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MarchingCubes.CommonTypes
{
    [DebuggerDisplay("P {X};{Y};{Z}")]
    public class Point : IEquatable<Point>
    {
        public Point(double x = 0, double y = 0, double z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.CalculatedValue = null;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        public double? CalculatedValue { get; set; }
        public double Get(int index)
        {
            if (index == AxissConsts.X)
            {
                return X;
            }
            else if (index == AxissConsts.Y)
            {
                return Y;
            }
            else
            {
                return Z;
            }
        }

        public Point NormalVector { get; set; }

        public void AddAndMergeNormal(Point p)
        {
            if (this.NormalVector == null)
            {
                this.NormalVector = new Point();
            }

            this.NormalVector.X += p.X;
            this.NormalVector.Y += p.Y;
            this.NormalVector.Z += p.Z;
        }

        public void Set(int index, double value)
        {
            if (index == AxissConsts.X)
            {
                this.X = value;
            }
            else if (index == AxissConsts.Y)
            {
                this.Y = value;
            }
            else
            {
                this.Z = value;
            }
        }

        public Point Clone()
        {
            return new Point(X, Y, Z)
            {
                CalculatedValue = CalculatedValue
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;


            if (object.ReferenceEquals(obj, this))
            {
                return true;
            }

            var var = (Point)obj;
            return var.X == this.X && var.Y == this.Y && var.Z == this.Z;
        }

        public bool Equals(Point other)
        {
            return Equals((object)other);
        }
    }
}
