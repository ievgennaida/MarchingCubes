using System;

namespace MarchingCubes.CommonTypes
{
    public class Variable : IVariable
    {
        public Variable()
        {
        }

        public Variable(String name, double value)
        {
            this.Name = name;
            this.Value = value;
        }
     
        public string Name { get; set; }
        public double Value { get; set; }

        public Variable(double value)
        {
            this.Value = value;
        }

        public bool IsConstant { get; set; }


        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static explicit operator double(Variable a)
        {
            if (a != null)
                return a.Value;
            else
                return 0;
        }

        public static implicit operator Variable(int value)
        {
            return new Variable((double)value);
        }

        public static implicit operator Variable(double value)
        {
            return new Variable(value);
        }

        public static double operator *(Variable a, Variable b)
        {
            return a.Value * b.Value;
        }

        public static double operator *(int a, Variable b)
        {
            return a * b.Value;
        }

        public Variable CloneVariable()
        {
            var var = new Variable(this.Value)
                          {
                              Name = this.Name,
                              Value=this.Value,
                              IsConstant=this.IsConstant
                          };
            return var;
        }

        public void GenerateRandomName()
        {
            this.Name = Guid.NewGuid().ToString();
        }

        public static Variable Convert(Variable inVariable)
        {
            var Variable = new Variable();
            var toReturn = new Variable(Variable.Name, Variable.Value);
            //toReturn.Type = Variable.Type;
            //toReturn.Description = Variable.Description;
            return toReturn;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            Variable var = (Variable)obj;
            return var.Value == this.Value;
        }
    }
}


