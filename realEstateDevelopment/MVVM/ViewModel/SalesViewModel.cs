using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using realEstateDevelopment.Core;
using System;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;

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
        #endregion
        #region Commands
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
            OpenAddNewSaleCommand = new RealyCommand(o =>
            {
                AddNewSaleRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
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
            throw new NotImplementedException();
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
