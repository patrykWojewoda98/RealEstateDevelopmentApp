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

        public override async Task ApplyFiltersAsync()
        {
            var filtered = List.Where(item =>
                (item.Area >= MinArea) &&
                (item.Area <= MaxArea) &&
                (item.Floor >= MinFloor) &&
                (item.Floor <= MaxFloor) &&
                (item.RoomCount >= MinRoomsNumber) &&
                (item.RoomCount <= MaxRoomsNumber) &&
                (string.IsNullOrWhiteSpace(Status) ||
                 (item.Status != null && item.Status.IndexOf(Status, StringComparison.OrdinalIgnoreCase) >= 0))).ToList();


            
            List.Clear();

            foreach (var item in filtered)
            {
                List.Add(item);
            }
        }

        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is ApartmentsEntitiesForView selectedApartment)
            {
                var modal = new DeleteApartmentModalView(); // Modal bez argumentów
                modal.DataContext = new DeleteApartmentModalViewModel(
                    realEstateEntities.Apartments.First(a => a.ApartmentID == selectedApartment.ApartmentID)
                );

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