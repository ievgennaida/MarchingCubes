using MarchingCubes.GraphicTypes;
using System;

namespace MarchingCubes.CommonTypes
{
    /// <summary>
    /// Type to present the max and min bounds 
    /// in which we found our gradient descent results 
    /// </summary>
    public class Region3D
    {
        public Region3D() { }

        public Region3D(double x, double maxX, double y, double maxY, double z = 0, double maxZ = 0)
        {
            this.MinX = x;
            this.MaxX = maxX;
            this.MinY = y;
            this.MaxY = maxY;
            this.MinZ = z;
            this.MaxZ = maxZ;
        }

        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MinZ { get; set; }

        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double MaxZ { get; set; }

        public double Width { get { return GetLenght(MinX, MaxX); } }
        public double Height { get { return GetLenght(MinY, MaxY); } }
        public double Depth { get { return GetLenght(MinZ, MaxZ); } }

        public static double GetLenght(double one, double two)
        {
            return Math.Sqrt(Math.Pow((two - one), 2));
        }

        public Point Center
        {
            get
            {
                return new Point(MinX + Width / 2, MinY + Height / 2, MinZ + Depth / 2);
            }
        }

        /// <summary>
        /// Region Will Resized (will grow up) if empty
        /// </summary>
        public void GrowIfEmpty()
        {
            if (Width == 0)
            {
                MinX -= 1;
                MaxX += 1;
            }
            if (Height == 0)
            {
                MinY -= 1;
                MaxY += 1;
            }
            if (Depth == 0)
            {
                MinZ -= 1;
                MaxZ += 1;
            }
        }


        /// <summary>
        /// Make the Width=Height=Depth
        /// </summary>
        public void MakeProportional()
        {
            if (Width - Height - Depth != 0)//not equals
            {
                //Lets make proportional width and height
                if (Width > Height)
                {
                    var toAdd = (Width - Height) / 2;
                    this.MinY -= toAdd;
                    this.MaxY += toAdd;
                }
                else
                {
                    var toAdd = (Height - Width) / 2;
                    this.MinX -= toAdd;
                    this.MaxX += toAdd;
                }
                //Lets make proportional depth
                //Now the Height==Width
                if (Depth > Height)
                {
                    var toAdd = (Depth - Height) / 2;
                    this.MinY -= toAdd;//because they are equals
                    this.MaxY += toAdd;
                    this.MinX -= toAdd;
                    this.MaxX += toAdd;
                }
                else
                {
                    var toAdd = (Height - Depth) / 2;
                    this.MinZ -= toAdd;
                    this.MaxZ += toAdd;
                }

            }
        }
        /// <summary>
        /// Get all region corners
        /// 4-5-6-7
        /// 0-1-2-3
        /// </summary>
        public Point[] GetCorners()
        {
            var points = new Point[8];
            points[0] = new Point(MinX, MinY, MinZ);
            points[1] = new Point(MaxX, MinY, MinZ);
            points[2] = new Point(MaxX, MinY, MaxZ);
            points[3] = new Point(MinX, MinY, MaxZ);
            points[4] = new Point(MinX, MaxY, MinZ);
            points[5] = new Point(MaxX, MaxY, MinZ);
            points[6] = new Point(MaxX, MaxY, MaxZ);
            points[7] = new Point(MinX, MaxY, MaxZ);
            return points;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != this.GetType())
                return false;
            var objc = (Region3D)obj;

            if (Object.ReferenceEquals(obj, this))
                return true;

            return objc.MaxX == this.MaxX && objc.MaxY == this.MaxY && objc.MaxZ == this.MaxZ && objc.MinX == this.MinX && objc.MinY == this.MinY && objc.MinZ == this.MinZ;
        }
        public static bool operator ==(Region3D r1, Region3D r2)
        {
            if (Object.ReferenceEquals(r1, null))
                return false;
            return r1.Equals(r2);
        }
        public static bool operator !=(Region3D r1, Region3D r2)
        {
            return !(r1 == r2);
        }
    }
}
