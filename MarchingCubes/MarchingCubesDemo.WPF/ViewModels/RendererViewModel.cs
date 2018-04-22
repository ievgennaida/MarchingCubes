using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GradientDescentWPF.ViewModels
{
    public class RendererViewModel : BaseViewModel
    {
        public RendererViewModel()
        {
            this.ApplyLimitsCommand = new RelayCommand((o) =>
            {

            });
        }
        public string minX;

        public string MinX
        {
            get
            {
                return minX;
            }

            set
            {
                if (minX != value)
                {
                    this.minX = value;
                    OnPropertyChanged("MinX");
                }
            }
        }

        public string maxX;

        public string MaxX
        {
            get
            {
                return maxX;
            }

            set
            {
                if (maxX != value)
                {
                    this.maxX = value;
                    OnPropertyChanged("MaxX");
                }
            }
        }

        public string minY;

        public string MinY
        {
            get
            {
                return minY;
            }

            set
            {
                if (minY != value)
                {
                    this.minY = value;
                    OnPropertyChanged("MinY");
                }
            }
        }

        public string maxY;

        public string MaxY
        {
            get
            {
                return maxY;
            }

            set
            {
                if (maxY != value)
                {
                    this.maxY = value;
                    OnPropertyChanged("MaxY");
                }
            }
        }
        public string minZ;

        public string MinZ
        {
            get
            {
                return minZ;
            }

            set
            {
                if (minZ != value)
                {
                    this.minZ = value;
                    OnPropertyChanged("MinZ");
                }
            }
        }

        public string maxZ;

        public string MaxZ
        {
            get
            {
                return maxZ;
            }

            set
            {
                if (maxZ != value)
                {
                    this.maxZ = value;
                    OnPropertyChanged("MaxZ");
                }
            }
        }

        public string step;

        public string Step
        {
            get
            {
                return step;
            }

            set
            {
                if (step != value)
                {
                    this.step = value;
                    OnPropertyChanged("Step");
                }
            }
        }

        public ICommand ApplyLimitsCommand { get; set; }


        public void SetFunctionMode(int dimension)
        {

        }

        //InfoBoxVisibleCommand
    }
}
