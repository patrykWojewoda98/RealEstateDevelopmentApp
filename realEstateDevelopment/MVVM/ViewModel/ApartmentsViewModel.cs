using realEstateDevelopment.Core;

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
    public class ApartmentsViewModel : LoadAllViewModel<ApartmentsEntitiesForView>
    {
        #region Properties
        private ObservableCollection<ApartmentsEntitiesForView> _filteredList;
        public ObservableCollection<ApartmentsEntitiesForView> FilteredList
        {
            get => _filteredList;
            set
            {
                _filteredList = value;
                OnPropertyChanged(() => FilteredList);
            }
        }
        private ApartmentsEntitiesForView _selectedItem;
        public ApartmentsEntitiesForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        private int _ID;
        public int ID
        {
            get => _ID;
            set
            {
                _ID = value;
                OnPropertyChanged(() => ID);
            }
        }

        private decimal _minArea;
        public decimal MinArea
        {
            get => _minArea;
            set
            {
                _minArea = value;
                OnPropertyChanged(() => MinArea);
                ApplyFiltersAsync();
            }
        }

        private decimal _maxArea;
        public decimal MaxArea
        {
            get => _maxArea;
            set
            {
                _maxArea = value;
                OnPropertyChanged(() => MaxArea);
                ApplyFiltersAsync();
            }
        }

        private int _minFloor;
        public int MinFloor
        {
            get => _minFloor;
            set
            {
                _minFloor = value;
                OnPropertyChanged(() => MinFloor);
                ApplyFiltersAsync();
            }
        }

        private int _maxFloor;
        public int MaxFloor
        {
            get => _maxFloor;
            set
            {
                _maxFloor = value;
                OnPropertyChanged(() => MaxFloor);
                ApplyFiltersAsync();
            }
        }

        private int _minRoomsNumber;
        public int MinRoomsNumber
        {
            get => _minRoomsNumber;
            set
            {
                _minRoomsNumber = value;
                OnPropertyChanged(() => MinRoomsNumber);
                ApplyFiltersAsync();
            }
        }

        private int _maxRoomsNumber;
        public int MaxRoomsNumber
        {
            get => _maxRoomsNumber;
            set
            {
                _maxRoomsNumber = value;
                OnPropertyChanged(() => MaxRoomsNumber);
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
        public RealyCommand OpenAddNewApartmentCommand { get; set; }
        public RealyCommand ReloadCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }

        #endregion
        public event Action AddNewApartmentRequested;
        #region
        public ApartmentsViewModel()
            : base()
        {
            _maxFloor = realEstateEntities.Apartments.Max(a => a.Floor);
            _maxRoomsNumber = realEstateEntities.Apartments.Max(a => a.RoomCount);
            _maxArea = realEstateEntities.Apartments.Max(a => a.Area);
            LoadAsync();
            OpenAddNewApartmentCommand = new RealyCommand(o =>
            {
                AddNewApartmentRequested?.Invoke();
            });

            ReloadCommand = new RealyCommand(async o =>
            {
                ReloadAsync();
            });

            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
        }
        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from a in realEstateEntities.Apartments
                        join b in realEstateEntities.Buildings on a.BuildingID equals b.BuildingID
                        join p in realEstateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new ApartmentsEntitiesForView
                        {
                            ApartmentID = a.ApartmentID,
                            BuildingID = b.BuildingID,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                            ApartmentNumber = a.ApartmentNumber,
                            Floor = a.Floor,
                            Area = a.Area,
                            Status = a.Status,
                            RoomCount = a.RoomCount
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<ApartmentsEntitiesForView>(result);

        }

        public override Task ApplyFiltersAsync()
        {
            var filtered = List.Where(item =>
                (MinArea == 0 || item.Area >= MinArea) &&
                (MaxArea == 0 || item.Area <= MaxArea) &&
                (MinFloor == 0 || item.Floor >= MinFloor) &&
                (MaxFloor == 0 || item.Floor <= MaxFloor) &&
                (MinRoomsNumber == 0 || item.RoomCount >= MinRoomsNumber) &&
                (MaxRoomsNumber == 0 || item.RoomCount <= MaxRoomsNumber) &&
                (string.IsNullOrEmpty(Status) || item.Status.Contains(Status))
            );

            FilteredList = new ObservableCollection<ApartmentsEntitiesForView>(filtered);

            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item); 
            }

            return Task.CompletedTask;
        }

        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is ApartmentsEntitiesForView selectedApartment)
            {
                var modal = new DeleteApartmentModalView();
                DeleteApartmentModalViewModel deleteApartmentModalViewModel = new DeleteApartmentModalViewModel(
                                            realEstateEntities.Apartments.First(a => a.ApartmentID == selectedApartment.ApartmentID) );
                deleteApartmentModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteApartmentModalViewModel;
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