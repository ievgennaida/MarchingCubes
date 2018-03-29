using GradientDescent.CommonTypes;
using System;

namespace GradientDescent.Algoritms.GradientDescent
{
    public class GradientDescentAlgoritm
    {
        protected Derivative derivation;
        protected FunctionHolder function;
        public GradientDescentParams Parameters { get; set; }

        public GradientDescentAlgoritm(FunctionHolder function) :
            this(function, GradientDescentParams.DefaultParams(function.Dimension),
            new Derivative(function, DerivationAccuracy.High))
        {
        }

        public GradientDescentAlgoritm(FunctionHolder function, GradientDescentParams parameters)
            : this(function, parameters, new Derivative(function, DerivationAccuracy.High))
        {
        }

        public GradientDescentAlgoritm(FunctionHolder function, GradientDescentParams parameters, Derivative derivation)
        {
            this.function = function;
            this.Parameters = parameters;
            this.derivation = derivation;
        }


        public Trajectory GradientDescent()
        {
            var parameters = this.Parameters;
            if (function == null || derivation == null || parameters == null)
                throw new OperationCanceledException("Неможливо розпочати роботу. Не всі параметри вказано");

            var curPoint = parameters.StartPoint;
            var curBeta = parameters.Beta;
            var staticBeta = curBeta;
            double curAlpha = 0;
            bool pointChanged = false;

            Arguments antiGrad = derivation.GetAntiGradient(curPoint);
            double functionInCurPoint = function.Calculate(curPoint);
            double antiGradModule = antiGrad.Pow(2).GetSumm();
            var allPoints = new Trajectory();
            allPoints.Add(curPoint);
            for (; ; )
            {
                if (pointChanged)
                {
                    antiGrad = derivation.GetAntiGradient(curPoint);
                    functionInCurPoint = function.Calculate(curPoint);
                    antiGradModule = antiGrad.Pow(2).GetSumm();
                }
                Arguments nextPoint = curPoint + (antiGrad * curBeta);

                double functionInNextPoint = function.Calculate(nextPoint);

                if (!IsValidValue(antiGradModule))
                    return allPoints;

                //F(Xk)-F(xk=B*antigrad(xk))>=epsilon|antigrad(xk)|^2
                double toTest = antiGradModule * (curBeta * parameters.Epsilon);
                double toTestLeft = functionInCurPoint - functionInNextPoint;
                if (toTestLeft >= toTest)
                {
                    //add point it is normal
                    allPoints.Add(nextPoint);
                    double exitValue = (nextPoint - curPoint).GetModule();
                    if (exitValue < parameters.AccuracyEpsilon)
                    {
                        return allPoints; //exit
                    }
                    else
                    {
                        curPoint = nextPoint;
                        // Increase step
                        Arguments antiGradNew = derivation.GetAntiGradient(curPoint);
                        antiGrad = antiGradNew;
                        bool calculated = false;
                        Arguments newValue = null;
                        double funcResultNew = 0;
                        Arguments oldValue = null;
                        double funcResultOld = 0;
                        for (; ; )
                        {
                            if (calculated)
                            {
                                oldValue = newValue;
                                funcResultOld = funcResultNew;
                            }
                            else
                            {
                                oldValue = curPoint + (antiGradNew * curBeta);
                                funcResultOld = function.Calculate(oldValue);
                            }

                            var nextInfinity = Double.IsInfinity(curBeta * parameters.M);
                            if (!nextInfinity)
                            {
                                curBeta = curBeta * parameters.M;
                                // Step is too big.
                                newValue = curPoint + (antiGradNew * curBeta);
                                funcResultNew = function.Calculate(newValue);
                            }
                            calculated = true;

                            if (nextInfinity || funcResultOld <= funcResultNew)
                            {
                                curBeta = curBeta / parameters.M;
                                staticBeta = curBeta;
                                pointChanged = true;
                                curPoint = curPoint + (antiGrad * curBeta);
                                curAlpha = 0;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    pointChanged = false;

                    if (curAlpha == 0)
                        curAlpha = parameters.Alpha;
                    else
                        curAlpha = curAlpha * curAlpha;

                    curBeta = staticBeta * curAlpha;
                }
            }
        }

        public static bool IsValidValue(double value)
        {
            return !(double.IsInfinity(value) || double.IsNaN(value));
        }
    }
}