using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GradientDescentWPF.ViewModels.Base
{
    public class CollectionViewModel<T> : BaseViewModel
        where T : class
    {
        public T selectedItem;

        public T SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                if (selectedItem != value)
                {
                    this.selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        public ObservableCollection<T> items;

        public ObservableCollection<T> Items
        {
            get
            {
                return items;
            }

            set
            {
                if (items != value)
                {
                    this.items = value;
                    OnPropertyChanged("Items");
                }
            }
        }
    }

}
