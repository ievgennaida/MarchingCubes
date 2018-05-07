using MarchingCubes.Algoritms;
using MarchingCubes.Algoritms.InterpolationAlgoritms;
using MarchingCubes.CommonTypes;
using MarchingCubesDemo.WPF;
using MarchingCubesDemo.WPF.Trackball;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace GradientDescentWPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            FunctionsViewModel = new FunctionsListViewModel();
            FunctionsViewModel.SetFunctionCommand = new RelayCommand(SetFunction);

        }

        private TrackballUtils units;
        private Viewport3D viewPort;

        public void Initialize(TrackballUtils units, Viewport3D viewPort)
        {
            this.units = units;
            this.viewPort = viewPort;
            SetFunction(null);
        }

        public FunctionsListViewModel FunctionsViewModel { get; set; }
        public RendererViewModel RendererViewModel { get; set; }

        private void SetFunction(object parameter)
        {
            var function = FunctionsViewModel.SelectedItem;

            var step = function.Step;

            IinterpolationAlgoritm interpolation = new SimpleInterpolation();
            if (UseGoldenSection)
            {
                interpolation = new GoldenSectionSearch(function.Function);
            }

            //interpolation = new SimpleInterpolation(false);
            var marchingCubes = new MarchingCubes.Algoritms.MarchingCubes.MarchingCubesAlgorithm(function.Function, interpolation);
            var defaultSize = 10;
            var region = function.Region ?? new Region3D(0, defaultSize, 0, defaultSize, 0, defaultSize);
            var results = marchingCubes.Generate(region, step, function.CountorLine);

            viewPort.Children.Remove(gridModel);
            var converter = new ModelsConverter();
            if (ShowGrid)
            {
                var grid = converter.RenderGrid(results.Grid, DrawVertexIndexes, null);
                viewPort.Children.Add(grid);

                gridModel = grid;
            }

            var model = converter.RenderMesh(results.Triangles, UseMeshNormals, ShowNormals, ShowTwoSided);
            viewPort.Children.Remove(meshModel);
            viewPort.Children.Add(model);
            meshModel = model;

            this.ShowFunctionsPanel = false;
        }

        ModelVisual3D gridModel;
        ModelVisual3D meshModel;

        private void Refresh()
        {
            SetFunction(null);
        }

        public bool PanMode
        {
            get
            {
                return this.units.PanMode;
            }

            set
            {
                if (this.units.PanMode != value)
                {
                    this.units.PanMode = value;
                    OnPropertyChanged("PanMode");
                }
            }
        }


        public bool FreeLookupMode
        {
            get
            {
                return this.units.FreeLookupMode;
            }

            set
            {
                if (this.units.FreeLookupMode != value)
                {
                    this.units.FreeLookupMode = value;
                    OnPropertyChanged("FreeLookupMode");
      
                }
            }
        }

        public bool drawGrid;

        public bool DrawGrid
        {
            get
            {
                return drawGrid;
            }

            set
            {
                if (drawGrid != value)
                {
                    this.drawGrid = value;
                    OnPropertyChanged("DrawGrid");
                    Refresh();
                }
            }
        }

        public bool drawVertexIndexes;

        public bool DrawVertexIndexes
        {
            get
            {
                return drawVertexIndexes;
            }

            set
            {
                if (drawVertexIndexes != value)
                {
                    this.drawVertexIndexes = value;
                    OnPropertyChanged("DrawVertexIndexes");
                    Refresh();
                }
            }
        }

        public bool showGrid;

        public bool ShowGrid
        {
            get
            {
                return showGrid;
            }

            set
            {
                if (showGrid != value)
                {
                    this.showGrid = value;
                    OnPropertyChanged("ShowGrid");
                    Refresh();
                }
            }
        }


        public bool showTwoSided;

        public bool ShowTwoSided
        {
            get
            {
                return showTwoSided;
            }

            set
            {
                if (showTwoSided != value)
                {
                    this.showTwoSided = value;
                    OnPropertyChanged("ShowTwoSided");
                    Refresh();
                }
            }
        }
        public bool showNormals;

        public bool ShowNormals
        {
            get
            {
                return showNormals;
            }

            set
            {
                if (showNormals != value)
                {
                    this.showNormals = value;
                    OnPropertyChanged("ShowNormals");
                    Refresh();
                }
            }
        }

        public bool useMeshNormals;

        public bool UseMeshNormals
        {
            get
            {
                return this.useMeshNormals;
            }

            set
            {
                if (useMeshNormals != value)
                {
                    this.useMeshNormals = value;
                    OnPropertyChanged("UseMeshNormals");
                    Refresh();
                }
            }
        }


        public bool useMathInterpolation = true;

        public bool UseMathInterpolation
        {
            get
            {
                return this.useMathInterpolation;
            }

            set
            {
                if (this.useMathInterpolation != value)
                {
                    this.useMathInterpolation = value;
                    OnPropertyChanged("UseMathInterpolation");
                    Refresh();
                }
            }
        }

        public bool useGoldenSection;

        public bool UseGoldenSection
        {
            get
            {
                return this.useGoldenSection;
            }

            set
            {
                if (useGoldenSection != value)
                {
                    this.useGoldenSection = value;
                    OnPropertyChanged("UseGoldenSection");
                    Refresh();
                }
            }
        }

        public bool useGradientDescent;

        public bool UseGradientDescent
        {
            get
            {
                return this.useGradientDescent;
            }

            set
            {
                if (this.useGradientDescent != value)
                {
                    this.useGradientDescent = value;
                    OnPropertyChanged("UseGradientDescent");
                    Refresh();
                }
            }
        }

        public bool showFunctionsPanel;

        public bool ShowFunctionsPanel
        {
            get
            {
                return this.showFunctionsPanel;
            }

            set
            {
                if (this.showFunctionsPanel != value)
                {
                    this.showFunctionsPanel = value;
                    OnPropertyChanged("ShowFunctionsPanel");
                }
            }
        }
    }
}
