using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ReservationsViewModel : LoadAllViewModel<ReservationsEntityForView>
    {
        #region Properties
        private ReservationsEntityForView _selectedItem;
        public ReservationsEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        private DateTime? _filterReservationDateFrom;
        public DateTime? FilterReservationDateFrom
        {
            get => _filterReservationDateFrom;
            set
            {
                _filterReservationDateFrom = value;
                OnPropertyChanged(() => FilterReservationDateFrom);
                ApplyFiltersAsync();
            }
        }

        private DateTime? _filterReservationDateTo;
        public DateTime? FilterReservationDateTo
        {
            get => _filterReservationDateTo;
            set
            {
                _filterReservationDateTo = value;
                OnPropertyChanged(() => FilterReservationDateTo);
                ApplyFiltersAsync();
            }
        }

        private string _filterBuildingNumber;
        public string FilterBuildingNumber
        {
            get => _filterBuildingNumber;
            set
            {
                _filterBuildingNumber = value;
                OnPropertyChanged(() => FilterBuildingNumber);
                ApplyFiltersAsync();
            }
        }

        private string _filterClientName;
        public string FilterClientName
        {
            get => _filterClientName;
            set
            {
                _filterClientName = value;
                OnPropertyChanged(() => FilterClientName);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<ReservationsEntityForView> _filteredList;
        public ObservableCollection<ReservationsEntityForView> FilteredList
        {
            get => _filteredList;
            set
            {
                _filteredList = value;
                OnPropertyChanged(() => FilteredList);
            }
        }
        #endregion
        #region Commands
        public RealyCommand ApplyFiltersCommand { get; set; }
        public RealyCommand OpenAddNewReservationCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewReservationRequested;
        #endregion

        #region Constructor
        public ReservationsViewModel()
            :base()
        {
            OpenAddNewReservationCommand = new RealyCommand(o =>
            {
                AddNewReservationRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
        }
        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from r in realEstateEntities.Reservations
                        join c in realEstateEntities.Clients on r.ClientID equals c.ClientID
                        join a in realEstateEntities.Apartments on r.ApartmentID equals a.ApartmentID
                        join b in realEstateEntities.Buildings on a.BuildingID equals b.BuildingID
                        join p in realEstateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new ReservationsEntityForView
                        {
                            Id = r.ReservationID,
                            ApartmentNumber = a.ApartmentNumber,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                            ReservationDate = r.ReservationDate,
                            ClientName = c.FirstName,
                            ClientSurname = c.LastName,
                            ClientPhoneNumber = c.PhoneNumber,
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<ReservationsEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            FilteredList = new ObservableCollection<ReservationsEntityForView>(
                List.Where(r =>
                    (!FilterReservationDateFrom.HasValue || r.ReservationDate >= FilterReservationDateFrom) &&
                    (!FilterReservationDateTo.HasValue || r.ReservationDate <= FilterReservationDateTo) &&
                    (string.IsNullOrEmpty(FilterBuildingNumber) || r.BuildingNumber.Contains(FilterBuildingNumber)) &&
                    (string.IsNullOrEmpty(FilterClientName) || r.ClientName.Contains(FilterClientName))
                ));
            List.Clear();
            foreach (var item in FilteredList) 
            {
                List.Add(item);
            }
            return Task.CompletedTask;
        }
        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is ReservationsEntityForView selected)
            {
                var modal = new DeleteReservationModalView();
                DeleteReservationModalViewModel deleteReservationModalViewModel = new DeleteReservationModalViewModel(
                                            realEstateEntities.Reservations.First(r => r.ReservationID == SelectedItem.Id));
                deleteReservationModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteReservationModalViewModel;
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
