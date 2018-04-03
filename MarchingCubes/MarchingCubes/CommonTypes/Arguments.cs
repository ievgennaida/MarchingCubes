
using MarchingCubes;
using MarchingCubes.CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MarchingCubes.CommonTypes
{
    [DebuggerDisplay("P {X},{Y},{Z}")]
    public class Arguments : List<Variable>
    {
        public Arguments(double x, double y, double z = 0)
            : base(new Variable[] 
            { 
                new Variable(x) { Name = nameof(AxissConsts.X) },
                new Variable(y) { Name = nameof(AxissConsts.Y) },
                new Variable(z) { Name = nameof(AxissConsts.Z) }
            }) { }

        public Arguments(params Variable[] variables)
            : base(variables)
        {
            SetNames(this);
        }

        public Arguments(IEnumerable<Variable> variables)
            : base(variables)
        {
            SetNames(this);
        }

        public Arguments()
        {

        }

        public void Revert()
        {
            foreach (var item in this)
            {
                item.Value = -item.Value;
            }
        }

        public void Add(double a)
        {
            this.Add(new Variable(a));
        }

        public override string ToString()
        {
            return String.Join(" ", this.Select(p => p.Value.ToString()).ToArray());
        }

        public Arguments Pow(int q)
        {
            Arguments toReturn = this.CloneArguments();
            foreach (var temp in toReturn)
            {
                for (int i = 0; i < q - 1; i++)
                {
                    temp.Value *= temp.Value;
                }
            }
            return toReturn;
        }

        public double GetModule()
        {
            return Math.Sqrt(this.Pow(2).GetSumm());
        }

        public double GetSumm()
        {
            double result = 0;
            foreach (var item in this)
            {
                result += item.Value;
            }
            return result;
        }

        public double X
        {
            get { return this[AxissConsts.X].Value; }
            set { this[AxissConsts.X].Value = value; }
        }

        public double Y
        {
            get { return this[AxissConsts.Y].Value; }
            set { this[AxissConsts.Y].Value = value; }
        }

        public double Z
        {
            get { return this[AxissConsts.Z].Value; }
            set { this[AxissConsts.Z].Value = value; }
        }

        public Arguments CloneArguments()
        {
            var toRet = new Arguments();
            foreach (var item in this)
            {
                toRet.Add(item.CloneVariable());
            }
            return toRet;
        }

        public static Arguments operator +(Arguments args, double h)
        {
            Arguments toReturn = args.CloneArguments();
            foreach (var temp in toReturn)
            {
                if (!temp.IsConstant)
                    temp.Value += h;
            }
            return toReturn;
        }

        public static Arguments operator -(Arguments args, Arguments h)
        {
            if (args.Count != h.Count)
                return args;

            int index = 0;
            Arguments toReturn = args.CloneArguments();
            foreach (var temp in toReturn)
            {
                if (!temp.IsConstant)
                    temp.Value -= h[index].Value;
                index++;
            }
            return toReturn;
        }

        public static Arguments operator +(Arguments args, Arguments h)
        {
            if (args.Count != h.Count)
                return args;

            int index = 0;
            Arguments toReturn = args.CloneArguments();
            foreach (var temp in toReturn)
            {
                if (!temp.IsConstant)
                    temp.Value += h[index].Value;
                index++;
            }
            return toReturn;
        }

        public override int GetHashCode()
        {
            int value = 0;

            foreach (var item in this)
            {
                value ^= item.GetHashCode();
            }
            return value;
        }
        public static Arguments operator -(Arguments args, double h)
        {
            Arguments toReturn = args.CloneArguments();
            foreach (var temp in toReturn)
            {
                if (!temp.IsConstant)
                    temp.Value -= h;
            }
            return toReturn;
        }

        public static Arguments operator -(double args, Arguments h)
        {
            return h - args;
        }

        public static Arguments operator *(Arguments args, double h)
        {
            Arguments toReturn = args.CloneArguments();
            foreach (var temp in toReturn)
            {
                temp.Value *= h;
            }
            return toReturn;
        }

        public static Arguments CreateEmptyArgs(int dimension)
        {
            Arguments args = new Arguments();
            for (int i = 0; i < dimension; i++)
            {
                args.Add(0);
            }
            SetNames(args);
            return args;
        }
        public void SetNames()
        {
            Arguments.SetNames(this);
        }
        public static void SetNames(Arguments args)
        {
            if (args.Count > AxissConsts.X)
                args[AxissConsts.X].Name = nameof(AxissConsts.X);
            if (args.Count > AxissConsts.Y)
                args[AxissConsts.Y].Name = nameof(AxissConsts.Y);
            if (args.Count > AxissConsts.Z)
                args[AxissConsts.Z].Name = nameof(AxissConsts.Z);
        }

        public static Arguments Convert(List<Variable> Variable)
        {
            var args = new Arguments();
            foreach (var lex in Variable)
            {
                args.Add(lex);
            }
            return args;
        }

        public static Region3D GetRegion(List<Arguments> arguments)
        {
            Region3D region = new Region3D();
            region.MinX = region.MinY = region.MinZ = float.MaxValue;
            region.MaxX = region.MaxY = region.MaxZ = float.MinValue;

            foreach (var item in arguments)
            {
                if (item.Count > AxissConsts.X)
                {
                    var value = item[AxissConsts.X].Value;
                    region.MinX = Math.Min(region.MinX, value);
                    region.MaxX = Math.Max(region.MaxX, value);
                }
                if (item.Count > AxissConsts.Y)
                {

                    var value = item[AxissConsts.Y].Value;
                    region.MinY = Math.Min(region.MinY, value);
                    region.MaxY = Math.Max(region.MaxY, value);
                }
                else
                    region.MinY = region.MaxY = 0;

                if (item.Count > AxissConsts.Z)
                {
                    var value = item[AxissConsts.Z].Value;
                    region.MinZ = Math.Min(region.MinZ, value);
                    region.MaxZ = Math.Max(region.MaxZ, value);
                }
                else
                    region.MinZ = region.MaxZ = 0;
            }

            return region;
        }

        public static Region3D GetRegion(double additionalRegionPecents, List<Arguments> arguments)
        {
            var region = GetRegion(arguments);
            region.MakeProportional();
            //region.ExpandRegionByPercents(additionalRegionPecents);
            return region;
        }

        public List<Variable> ConvertToVariables()
        {
            List<Variable> toReturn = new List<Variable>();
            foreach (var temp in this)
            {
                toReturn.Add(temp);
            }

            return toReturn;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            Arguments arg = (Arguments)obj;

            if (arg.Count != this.Count)
                return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (!arg[i].Equals(this[i]))
                    return false;
            }

            return true;
        }

        /*hell, only for marching cubes algoritm*/
        public Point NormalVector { get; set; }
    }
}


