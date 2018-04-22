using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

namespace MarchingCubesDemo.WPF.Trackball
{
    /// <summary>
    ///     Trackball is a utility class which observes the mouse events
    ///     on a specified FrameworkElement and produces a Transform3D
    ///     with the resultant rotation and scale.
    /// 
    ///     Example Usage:
    /// 
    ///         Trackball trackball = new Trackball();
    ///         trackball.EventSource = myElement;
    ///         myViewport3D.Camera.Transform = trackball.Transform;
    /// 
    ///     Because Viewport3Ds only raise events when the mouse is over the
    ///     rendered 3D geometry (as opposed to not when the mouse is within
    ///     the layout bounds) you usually want to use another element as 
    ///     your EventSource.  For example, a transparent border placed on
    ///     top of your Viewport3D works well:
    ///     
    ///         <Grid>
    ///           <ColumnDefinition />
    ///           <RowDefinition />
    ///           <Viewport3D Name="myViewport" ClipToBounds="True" Grid.Row="0" Grid.Column="0" />
    ///           <Border Name="myElement" Background="Transparent" Grid.Row="0" Grid.Column="0" />
    ///         </Grid>
    ///     
    ///     NOTE: The Transform property may be shared by multiple Cameras
    ///           if you want to have auxilary views following the trackball.
    /// 
    ///           It can also be useful to share the Transform property with
    ///           models in the scene that you want to move with the camera.
    ///           (For example, the Trackport3D's headlight is implemented
    ///           this way.)
    /// 
    ///           You may also use a Transform3DGroup to combine the
    ///           Transform property with additional Transforms.
    /// </summary>
    public class TrackballUtils
    {
        private FrameworkElement _eventSource;
        private Point _previousPosition2D;
        private Vector3D _previousPosition3D = new Vector3D(0, 0, 1);

        private Transform3DGroup _transform;
        private ScaleTransform3D _scale = new ScaleTransform3D();
        private AxisAngleRotation3D _rotation = new AxisAngleRotation3D();
        private TranslateTransform3D _translate = new TranslateTransform3D();

        public TrackballUtils()
        {
            _transform = new Transform3DGroup();
            _transform.Children.Add(_scale);
            _transform.Children.Add(new RotateTransform3D(_rotation));
            _transform.Children.Add(_translate);
            this.PanMode = true;
        }

        /// <summary>
        ///     A transform to move the camera or scene to the trackball's
        ///     current orientation and scale.
        /// </summary>
        public Transform3D Transform
        {
            get { return _transform; }
        }

        public ProjectionCamera Camera { get; set; }

        #region Event Handling

        /// <summary>
        ///     The FrameworkElement we listen to for mouse events.
        /// </summary>
        public FrameworkElement EventSource
        {
            get { return _eventSource; }

            set
            {
                if (_eventSource != null)
                {
                    _eventSource.MouseDown -= this.OnMouseDown;
                    _eventSource.MouseUp -= this.OnMouseUp;
                    _eventSource.MouseWheel -= _eventSource_MouseWheel;
                    _eventSource.MouseMove -= this.OnMouseMove;
                    Mouse.Capture(_eventSource, CaptureMode.None);
                }

                _eventSource = value;
                _eventSource.MouseWheel += _eventSource_MouseWheel;
                _eventSource.MouseDown += this.OnMouseDown;
                _eventSource.MouseUp += this.OnMouseUp;
                _eventSource.MouseMove += this.OnMouseMove;
            }
        }

        void _eventSource_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Zoom(-1 * e.Delta / 2);
            //Zoom(e.Delta);
        }

        // Fix bug with a click.
        private bool isFirstEvent = true;
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isFirstEvent = true;
                Mouse.Capture(EventSource, CaptureMode.Element);
                _previousPosition2D = e.GetPosition(EventSource);
                _previousPosition3D = ProjectToTrackball(
                    EventSource.ActualWidth,
                    EventSource.ActualHeight,
                    _previousPosition2D);
            }
        }


        public bool PanMode { get; set; }

        public bool FreeLookupMode { get; set; }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                isFirstEvent = false;
                Mouse.Capture(EventSource, CaptureMode.None);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Fixed bug with flickering. First move cannot be outside of bounds.
            Point currentPosition = e.GetPosition(EventSource);
            if (isFirstEvent && (currentPosition.X < 0 || currentPosition.Y < 0))
            {
                isFirstEvent = false;
                return;
            }

            isFirstEvent = false;
            if (e.LeftButton == MouseButtonState.Pressed && ( (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) )|| FreeLookupMode))
            {
                Look(currentPosition);
            }
            else if (e.LeftButton == MouseButtonState.Pressed && PanMode)
            {
                Pan(currentPosition);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                Zoom(currentPosition);
            }

            _previousPosition2D = currentPosition;
        }

        #endregion Event Handling

        public void Look(Point currentPosition)
        {
            Vector3D currentPosition3D = ProjectToTrackball(EventSource.ActualWidth, EventSource.ActualHeight, currentPosition);
            //currentPosition3D = new Vector3D(_previousPosition3D.X, _previousPosition3D.Y, currentPosition3D.Z);
            Vector3D axis = Vector3D.CrossProduct(_previousPosition3D, currentPosition3D);
            double angle = Vector3D.AngleBetween(_previousPosition3D, currentPosition3D);

            //axis.X = 0;
            //axis.Z = 0;
            //axis.Y = 0;
            if (axis.Length == 0)
            {
                _previousPosition3D = currentPosition3D;
                return;
            }

            Quaternion delta = new Quaternion(axis, -angle);

            // Get the current orientantion from the RotateTransform3D
            AxisAngleRotation3D r = _rotation;
            Quaternion q = new Quaternion(_rotation.Axis, _rotation.Angle);

            // Compose the delta with the previous orientation
            q *= delta;

            // Write the new orientation back to the Rotation3D
            _rotation.Axis = q.Axis;
            //_rotation.Axis = new Vector3D(q.Axis.X, _rotation.Axis.Y, _rotation.Axis.Z);
            _rotation.Angle = q.Angle;

            _previousPosition3D = currentPosition3D;
        }

        public void Pan(Point currentPosition)
        {
            Vector change = Point.Subtract(_previousPosition2D, currentPosition);

            Camera.Position = new Point3D(Camera.Position.X + change.X * .004, Camera.Position.Y - change.Y * .004, Camera.Position.Z);

            //_translate.OffsetX += changeVector.X * .004;
            //_translate.OffsetY -= changeVector.Y * .004;
            //_translate.OffsetZ += changeVector.Z * .004;

            _previousPosition3D = ProjectToTrackball(EventSource.ActualWidth, EventSource.ActualHeight, currentPosition);//save last 3d position
        }

        private Vector3D ProjectToTrackball(double width, double height, Point point)
        {
            var x = point.X / (width / 2);    // Scale so bounds map to [0,0] - [2,2]
            var y = point.Y / (height / 2);

            x = x - 1;                           // Translate 0,0 to the center
            y = 1 - y;                           // Flip so +Y is up instead of down

            var z2 = 1 - x * x - y * y;       // z^2 = 1 - x^2 - y^2
            var z = z2 > 0 ? Math.Sqrt(z2) : 0;

            return new Vector3D(x, y, z);
        }

        public void Zoom(Point currentPosition)
        {
            var yDelta = currentPosition.Y - _previousPosition2D.Y;
            Zoom(yDelta);
        }

        public void Zoom(double yDelta)
        {
            double scale = Math.Exp(yDelta / 100);    // e^(yDelta/100) is fairly arbitrary.
            //Camera.Position = new Point3D(Camera.Position.X, Camera.Position.Y, Camera.Position.Z - yDelta / 100D);
            _scale.ScaleX *= scale;
            _scale.ScaleY *= scale;
            _scale.ScaleZ *= scale;
        }
    }
}
