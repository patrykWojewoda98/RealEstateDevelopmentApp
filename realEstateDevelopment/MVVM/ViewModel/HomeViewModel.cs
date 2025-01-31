using System.ComponentModel;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private string _welcome;
        public string Welcome
        {
            get => _welcome;
            set
            {
                _welcome = value;
                OnPropertyChanged(nameof(Welcome));
            }
        }

        public HomeViewModel()
        {
            if (MainViewModel.employee != null)
            {
                Welcome = "Witaj " + MainViewModel.employee.FirstName + " " + MainViewModel.employee.LastName + "!";
            }
            else
            {
                Welcome = "Witaj użytkowniku!";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
