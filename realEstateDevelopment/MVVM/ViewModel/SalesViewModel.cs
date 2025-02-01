using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using realEstateDevelopment.Core;
using System;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class SalesViewModel : LoadAllViewModel<SalesEntityForView>
    {
        #region Properties
        private SalesEntityForView _selectedItem;
        public SalesEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        private decimal? _filterAmountFrom;
        public decimal? FilterAmountFrom
        {
            get => _filterAmountFrom;
            set
            {
                _filterAmountFrom = value;
                OnPropertyChanged(() => FilterAmountFrom);
                ApplyFiltersAsync();
            }
        }

        private decimal? _filterAmountTo;
        public decimal? FilterAmountTo
        {
            get => _filterAmountTo;
            set
            {
                _filterAmountTo = value;
                OnPropertyChanged(() => FilterAmountTo);
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

        private string _filterApartmentNumber;
        public string FilterApartmentNumber
        {
            get => _filterApartmentNumber;
            set
            {
                _filterApartmentNumber = value;
                OnPropertyChanged(() => FilterApartmentNumber);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<SalesEntityForView> _filteredList;
        public ObservableCollection<SalesEntityForView> FilteredList
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
        public RealyCommand OpenAddNewSaleCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewSaleRequested;
        #endregion

        #region Constructor
        public SalesViewModel()
            : base()
        {
            _filterAmountTo = realEstateEntities.Sales.Max(s => s.SalePrice);
            OpenAddNewSaleCommand = new RealyCommand(o =>
            {
                AddNewSaleRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
        }
        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from s in realEstateEntities.Sales
                        join c in realEstateEntities.Clients on s.ClientID equals c.ClientID
                        join a in realEstateEntities.Apartments on s.ApartmentID equals a.ApartmentID
                        join b in realEstateEntities.Buildings on a.BuildingID equals b.BuildingID
                        join p in realEstateEntities.Projects on b.ProjectID equals p.ProjectID
                        select new SalesEntityForView
                        {
                            Id = s.SaleID,
                            ApartmentNumber = a.ApartmentNumber,
                            BuildingNumber = b.BuildingNumber,
                            Address = p.Location,
                            SaleDate = s.SaleDate,
                            ClientName = c.FirstName,
                            ClientSurname = c.LastName,
                            SalePrice = s.SalePrice,
                            Status = s.Status,
                        };
            var result = await query.ToListAsync();
            List = new ObservableCollection<SalesEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            var filtered = List.Where(s =>
                (!FilterAmountFrom.HasValue || s.SalePrice >= FilterAmountFrom.Value) &&
                (!FilterAmountTo.HasValue || s.SalePrice <= FilterAmountTo.Value) &&
                (string.IsNullOrEmpty(FilterClientName) ||
                 (!string.IsNullOrEmpty(s.ClientName) && s.ClientName.Contains(FilterClientName))) &&
                (string.IsNullOrEmpty(FilterApartmentNumber) ||
                 (!string.IsNullOrEmpty(s.ApartmentNumber) && s.ApartmentNumber.Contains(FilterApartmentNumber)))
            );

            FilteredList = new ObservableCollection<SalesEntityForView>(filtered);

            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item);
            }

            return Task.CompletedTask;
        }

        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is SalesEntityForView selected)
            {
                var modal = new DeleteSaleModalView();
                DeleteSaleModalViewModel deleteSaleModalViewModel = new DeleteSaleModalViewModel(
                                            realEstateEntities.Sales.First(s => s.SaleID == SelectedItem.Id));
                deleteSaleModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteSaleModalViewModel;
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
