﻿using realEstateDevelopment.Core;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        #region Commands
        public RealyCommand HomeViewCommand { get; set; }
        public RealyCommand BuildingsViewCommand { get; set; }
        public RealyCommand ClientsViewCommand { get; set; }
        #endregion


        #region Properties
        public HomeViewModel HomeVM { get; set; }
        public BuildingsViewModel BuildingsVM { get; set; }
        public ClientViewModel ClientVM { get; set; }
        private object _currentView;
        #endregion

        
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
            HomeVM = new HomeViewModel();
            BuildingsVM = new BuildingsViewModel();
            ClientVM = new ClientViewModel();

            BuildingsVM.AddNewBuildingRequested += OnAddNewBuildingRequested;
            ClientVM.AddNewClientRequested += OnAddNewClientRequested;

            

            CurrentView = HomeVM;

            HomeViewCommand = new RealyCommand(o =>
            {
                CurrentView = HomeVM;
            });

            BuildingsViewCommand = new RealyCommand(o =>
            {
                CurrentView = BuildingsVM;
            });

            ClientsViewCommand = new RealyCommand(o =>
            {
                CurrentView = ClientVM;
            });
        }

        #region Helpers
        private void OnAddNewBuildingRequested()
        {
            var addNewBuildingVM = new AddNewBuildingViewModel();
            addNewBuildingVM.RequestClose += (sender, args) =>
            {
                CurrentView = BuildingsVM;
            };
            CurrentView = addNewBuildingVM;
        }

        private void OnAddNewClientRequested()
        {
            var addNewClientVM = new AddNewClientViewModel();
            addNewClientVM.RequestClose += (sender, args) =>
            {
                CurrentView = BuildingsVM;
            };
            CurrentView = addNewClientVM;
        }
        #endregion
    }
}
