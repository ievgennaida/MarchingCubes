using GradientDescentWPF.ViewModels.Base;
using MarchingCubes.CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GradientDescentWPF.ViewModels
{
    public class FunctionsListViewModel : CollectionViewModel<FunctionData>
    {
        public FunctionsListViewModel()
        {
            this.Items = new System.Collections.ObjectModel.ObservableCollection<FunctionData>();
            this.Items.Add(new FunctionData()
            {
                Description = "x^2/3+y^2/24+z^2/21",
                Function = (args) =>
                {
                    var x = args.X;
                    var y = args.Y;
                    var z = args.Z;
                    return ((x * x) / 3) + ((y * y) / 24) + ((z * z) / 21);
                }
            });
            this.Items.Add(new FunctionData()
            {
                Description = "(x-2)^2/3+(y-30)^2/24+(z-45)^2/2",
                Region = new Region3D(-5, 5, -5, 5, -5, 5),
                CountorLine = 0.5,
                Step = 0.25,
                Function = (args) =>
                {
                    var x = args.X - 2;
                    var y = args.Y - 30;
                    var z = args.Z - 45;
                    return x * x / 3 + y * y / 24 + z * z / 2;
                }
            });
            this.Items.Add(new FunctionData()
            {
                Description = "Two - sheeted hyperboloid:" + Environment.NewLine + "x^2 - 3y^2 - z^2",
                Region = new Region3D(-5, 5, -5, 5, -5, 5),
                CountorLine = 0.5,
                Step = 0.25,
                Function = (args) =>
                {
                    var x = args.X;
                    var y = args.Y;
                    var z = args.Z;
                    return x * x + 3 * y * y - z * z;
                }
            });

     
            this.Items.Add(new FunctionData()
            {
                CountorLine = 1,
                Step = 0.5,
                Region = new Region3D(-1.5, 1.5, -1.5, 1.5, -1.5, 1.5),
                Description = "Sphere:" + Environment.NewLine + "(x-1.5)^2+(y-1.5)^2+(z-1.5)^2-1",
                Function = (args) =>
                {
                    var x = args.X;
                    var y = args.Y;
                    var z = args.Z;

                    var x0 = 0;
                    var y0 = 0;
                    var z0 = 0;
                    var radius = 1;
                    return Math.Pow((x - x0), 2) + Math.Pow((y - y0), 2) + Math.Pow((z - z0), 2) - Math.Pow(radius, 2);
                }
            });

            this.SelectedItem = this.Items.LastOrDefault();

        }


        public ICommand SetFunctionCommand { get; set; }
    }
}
