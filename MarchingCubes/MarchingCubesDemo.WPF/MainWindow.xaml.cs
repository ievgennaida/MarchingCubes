using MarchingCubes.Algoritms.CountorLines;
using MarchingCubes.CommonTypes;
using MarchingCubesDemo.WPF.Trackball;
using Petzold.Media3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarchingCubesDemo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TrackballUtils trackball = new TrackballUtils();
        public MainWindow()
        {
            InitializeComponent();
            DrawGridLines();


            this.Camera.Transform = trackball.Transform;
            this.Camera.NearPlaneDistance = 0;
            this.Camera.Width = 4;

            this.Loaded += OnLoaded;
 //           graph3DContainer = new Graph3DContainer(Viewport);
            // listBoxPresenter = new CountorLineListBoxPresenter(tbCounturValue, lbContourLinesList);
            this.Headlight.Transform = trackball.Transform;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Viewport3Ds only raise events when the mouse is over the rendered 3D geometry.
            // In order to capture events whenever the mouse is over the client are we use a
            // same sized transparent Border positioned on top of the Viewport3D.
            trackball.EventSource = container;
            trackball.Camera = this.Camera;
        }

        private void DrawGridLines()
        {
            var modelToAdd = new ModelVisual3D();
            var size = 10;
            var step = 5;
            var marchingCubes = new AdaptiveMarchingCubes(null);
            var region = new Region3D(0, size, 0, size, 0, size);
            var cubes = marchingCubes.BuildGrid(region, step);

            var lines = new List<GridLine>();
            foreach (var cube in cubes)
            {
                foreach (var edge in cube.Edges)
                {
                    if(edge==null)
                    {

                    }
                    if (!lines.Contains(edge))
                    {
                        lines.Add(edge);
                        modelToAdd.Children.Add(GetLine(edge.Point1, edge.Point2, Colors.Red));
                    }
                }
            }


            viewport.Children.Add(modelToAdd);
        }

        private WireLine GetLine(Arguments point1, Arguments point2, Color color)
        {
            var line = new WireLine();
            line.Point1 = new Point3D(point1.X, point1.Y, point1.Z);
            line.Point2 = new Point3D(point2.X, point2.Y, point2.Z);
            line.Thickness = 1;
            line.Color = color;
            return line;
        }
    }
}
