using GradientDescent.CommonTypes;
using System;
using System.Collections.Generic;

namespace GradientDescent
{
    public delegate double Function(Arguments args);

    public interface IVariable
    {
    }

    public class FunctionHolder : ICloneable
    {
        private int dimension;
        private Function function;
        private bool setFunction;
        public FunctionHolder(Function function, int dimension)
        {
            setFunction = true;
            this.function = function;
            this.dimension = dimension;
        }

        //public FunctionHolder(AnalysisResults analysisResults, Function function, int dimension)
        //{
        //    setFunction = true;
        //    this.function = function;
        //    this.dimension = dimension;
        //    AnalysisResults = analysisResults;
        //}

        //public FunctionHolder(AnalysisResults analysisResults, StackMachine stackMachine)
        //{
        //    setFunction = false;
        //    this.stackMachine = stackMachine;
        //    AnalysisResults = analysisResults;
        //    if (AnalysisResults.SyntaxResult != null)
        //    {
        //        dimension = AnalysisResults.SyntaxResult.Variables.Count;
        //    }
        //}

        public Function Function
        {
            get { return function; }
        }

        //public AnalysisResults AnalysisResults { get; set; }

        public int Dimension
        {
            get { return dimension; }
        }

        private bool imitateDimension;

        public bool ImitateDimension
        {
            get { return imitateDimension; }
            set
            {
                if (value == true && ImitationArguments == null)
                    throw new InvalidOperationException("ImitationArguments cannot be null. Run InitImitationDimension first");
                imitateDimension = value;
            }
        }
        /// <summary>
        /// Method to initialize mode of function calculation.
        /// <para>Used to calculate value of function with greater dimension than arguments count 
        /// All shortage of arguments will be taked from imitationArguments</para>
        /// </summary>
        public void InitImitateDimension(Arguments imitationArguments)
        {
            if (imitationArguments == null)
                throw new ArgumentNullException("imitationArguments");

            if (imitationArguments.Count != dimension)
                throw new ArgumentException("ImitationArguments must have dimension same like function than fun");

            ImitationArguments = imitationArguments;
            ImitateDimension = true;
        }

        public Arguments ImitationArguments { get; private set; }

        public double Calculate(params double[] arguments)
        {
            List<Variable> toInsert = new List<Variable>();
            foreach (var arg in arguments)
            {
                toInsert.Add(new Variable(arg));
            }
            return this.Calculate(new Arguments(toInsert));
        }

        public double Calculate(Arguments args)
        {
            if (ImitateDimension)
            {
                if (args.Count != dimension)
                {
                    var newArgs = ImitationArguments.CloneArguments();
                    int index = 0;
                    foreach (var arg in args)
                    {
                        newArgs[index] = arg.CloneVariable();
                        index++;
                    }
                    args = newArgs;
                }
            }

            if (setFunction)
            {
                return function(args);
            }
            else
            {
                if (args.Count != this.dimension)
                    throw new ArgumentException("Function dimension have to be " + this.dimension);

                if (args.Count > 0 && string.IsNullOrEmpty(args[0].Name))
                {
                    Arguments.SetNames(args);
                }

                return 0;
                //foreach (Variable arg in args)
                //{
                //    foreach (Variable var in AnalysisResults.SyntaxResult.Variables)
                //    {
                //        if (var.Name == arg.Name)
                //        {
                //            var.Value = arg.Value;
                //        }
                //    }
                //}

                //return stackMachine.OptimizeExpression(AnalysisResults.SyntaxResult.RpnResult, true)[0].Value;
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            return CloneFunctionHolder();
        }

        public FunctionHolder CloneFunctionHolder()
        {
            return new FunctionHolder(function, dimension);
        }
        #endregion
    }
}