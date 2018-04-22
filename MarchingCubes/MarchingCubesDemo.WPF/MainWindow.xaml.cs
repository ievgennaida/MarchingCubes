using GradientDescentWPF.ViewModels;
using MarchingCubes.Algoritms.MarchingCubes;
using MarchingCubes.CommonTypes;
using MarchingCubesDemo.WPF.Trackball;
using Petzold.Media3D;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MarchingCubesDemo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TrackballUtils trackball = new TrackballUtils();
        private MainWindowViewModel context;
        public MainWindow()
        {
            InitializeComponent();

            this.Camera.Transform = trackball.Transform;
            this.Headlight.Transform = trackball.Transform;

            // Viewport3Ds only raise events when the mouse is over the rendered 3D geometry.
            // In order to capture events whenever the mouse is over the client are we use a
            // same sized transparent Border positioned on top of the Viewport3D.
            trackball.EventSource = container;
            trackball.Camera = this.Camera;

            this.DataContext = context = new MainWindowViewModel();
            context.Initialize(trackball,viewport);
            //           graph3DContainer = new Graph3DContainer(Viewport);
            // listBoxPresenter = new CountorLineListBoxPresenter(tbCounturValue, lbContourLinesList);

        }

       
        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);
           // if (position.Y < 70 && position.Y != 0)
            {
                if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            //{
            //    if (!currentCube.HasValue)
            //    {
            //        currentCube = 0;
            //    }
            //    currentCube++;
            //    //currentCube = DrawGridLines(currentCube);
            //}
            //else if (e.Key == Key.Left)
            //{
            //    if (!currentCube.HasValue)
            //    {
            //        currentCube = 0;
            //    }

            //    currentCube--;
            // //   currentCube = DrawGridLines(currentCube);
            //}
            //else if (e.Key == Key.Enter)
            //{
            //    currentCube = null;
            // //   currentCube = DrawGridLines(currentCube);
            //}

            base.OnKeyDown(e);
        }



        private void btnMinimazeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);
            if (position.Y < 40 && position.Y != 0)
            {
                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                }
                else if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        double deltaZoom = 5;
        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
            trackball.Zoom(deltaZoom);
        }

        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {
            trackball.Zoom(-1 * deltaZoom);
        }
    }
}
