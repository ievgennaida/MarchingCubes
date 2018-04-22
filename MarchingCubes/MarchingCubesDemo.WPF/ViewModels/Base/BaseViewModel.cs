using System.ComponentModel;

namespace GradientDescentWPF.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                var newArgument = new PropertyChangedEventArgs(propertyName);
                handler(this, newArgument);
            }
        }
    }
}
