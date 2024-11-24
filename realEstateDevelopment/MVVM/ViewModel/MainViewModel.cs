using realEstateDevelopment.Core;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public HomeViewModel HomeVM { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            // Tworzenie instancji HomeViewModel
            HomeVM = new HomeViewModel();

            // Ustawienie CurrentView na HomeVM
            CurrentView = HomeVM;
        }
    }
}
