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
            DrawGridLines(currentCube);


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

        int? currentCube = null;
        private int? DrawGridLines(int? index)
        {
            var modelToAdd = new ModelVisual3D();
            var size = 15;
            var step = 5;
            var marchingCubes = new MarchingCubes.Algoritms.CountorLines.MarchingCubes(null);
            var region = new Region3D(0, size, 0, size, 0, size);
            var cubes = marchingCubes.BuildGrid(region, step);

            var lines = new List<GridLine>();
            //foreach (var cube in cubes)

            if (index.HasValue)
            {
                if (index <= 0)
                {
                    index = 0;
                }
                else if (cubes.Count <= index)
                {
                    index = 0;
                }
            }

            foreach (var cube in cubes)
            {
                var toUseCube = cube;
                if (index.HasValue)
                {
                    toUseCube = cubes[index.Value];
                }

                foreach (var edge in toUseCube.Edges)
                {
                    if (!lines.Contains(edge))
                    {
                        lines.Add(edge);
                        modelToAdd.Children.Add(GetLine(edge.Point1, edge.Point2, Colors.Red));
                    }
                }

                if (index.HasValue)
                {
                    int indexName = 0;
                    foreach (var vert in toUseCube.Vertex)
                    {
                        var model = new Petzold.Media3D.WireText() { Text = indexName.ToString(), };
                        model.Origin = new Point3D(vert.X, vert.Y, vert.Z);
                        model.FontSize = 1;
                        modelToAdd.Children.Add(model);
                        indexName++;
                    }
                    break;
                }
            }

            viewport.Children.Clear();
            viewport.Children.Add(modelToAdd);
            return index;


        }

        protected override void OnKeyDown(KeyEventArgs e)
        {

            if (e.Key == Key.Right)
            {
                if (!currentCube.HasValue)
                {
                    currentCube = 0;
                }
                currentCube++;
                currentCube = DrawGridLines(currentCube);
            }
            else if (e.Key == Key.Left)
            {
                if (!currentCube.HasValue)
                {
                    currentCube = 0;
                }

                currentCube--;
                currentCube = DrawGridLines(currentCube);
            }
            else if (e.Key == Key.Enter)
            {
                currentCube = null;
                currentCube = DrawGridLines(currentCube);
            }

            base.OnKeyDown(e);
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
