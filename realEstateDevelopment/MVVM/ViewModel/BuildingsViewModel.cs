using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class BuildingsViewModel : LoadAllViewModel<BuildingsEntityForView>
    {
        #region Properties
        private ObservableCollection<BuildingsEntityForView> _filteredList;
        public ObservableCollection<BuildingsEntityForView> FilteredList
        {
            get => _filteredList;
            set
            {
                _filteredList = value;
                OnPropertyChanged(() => FilteredList);
            }
        }
        private BuildingsEntityForView _selectedItem;
        public BuildingsEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(() => Address);
                ApplyFiltersAsync();
            }
        }

       

        private int _minFloors;
        public int MinFloors
        {
            get => _minFloors;
            set
            {
                _minFloors = value;
                OnPropertyChanged(() => MinFloors);
                ApplyFiltersAsync();
            }
        }

        private int _maxFloors;
        public int MaxFloors
        {
            get => _maxFloors;
            set
            {
                _maxFloors = value;
                OnPropertyChanged(() => MaxFloors);
                ApplyFiltersAsync();
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(() => Status);
                ApplyFiltersAsync();
            }
        }

        #endregion

        #region Commands
        public RealyCommand OpenAddNewBuildingCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewBuildingRequested;
        #endregion


        #region Constructor
        public BuildingsViewModel()
            : base()
        {
            _maxFloors = realEstateEntities.Buildings.Max(b => b.Floors);
            OpenAddNewBuildingCommand = new RealyCommand(o =>
            {
                AddNewBuildingRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
        }
        #endregion


        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from b in realEstateEntities.Buildings
                        join p in realEstateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new BuildingsEntityForView
                        {
                            BuildingID = b.BuildingID,
                            BuildingName = b.BuildingName,
                            ProjectID = b.ProjectID,
                            BuildingNumber = b.BuildingNumber,
                            Localization = p.Location,
                            Floors = b.Floors,
                            Status = b.Status,
                        };
            var result = await query.ToListAsync();
            List = new ObservableCollection<BuildingsEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            var filtered = List.Where(item =>
                (string.IsNullOrEmpty(Address) || item.Localization.Contains(Address)) &&
                (MinFloors == 0 || item.Floors >= MinFloors) &&
                (MaxFloors == 0 || item.Floors <= MaxFloors) &&
                (string.IsNullOrEmpty(Status) || item.Status.Contains(Status))
            );

            FilteredList = new ObservableCollection<BuildingsEntityForView>(filtered);

            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item);
            }

            return Task.CompletedTask;
        }

        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is BuildingsEntityForView selectedbuilding)
            {
                var modal = new DeleteBuildingModalView(); // Modal bez argumentów

                // tu dokończyć
                DeleteBuildingModalViewModel deleteBuildingModalViewModel = new DeleteBuildingModalViewModel(
                                        realEstateEntities.Buildings.First(b=> b.BuildingID == selectedbuilding.BuildingID)
                    );
                deleteBuildingModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteBuildingModalViewModel;

                modal.Show();

            }
        }


        private bool CanExecuteDeleteSelected(object parameter)
        {
            return SelectedItem != null; // Polecenie dostępne tylko, jeśli coś jest zaznaczone.
        }

        #endregion
    }
}