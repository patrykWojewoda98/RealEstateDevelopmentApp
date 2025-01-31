using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteSaleModalViewModel : BaseDataDeleter<Sales>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int Id
        {
            get => item.SaleID;
            set
            {
                item.SaleID = value;
                OnPropertyChanged(() => Id);
            }
        }

        public string ApartmentNumber
        {
            get => estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == item.ApartmentID)?.ApartmentNumber;
        }

        public string BuildingNumber
        {
            get => estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == item.ApartmentID).BuildingID).BuildingNumber;
        }

        public string Address
        {
            get => estateEntities.Projects.FirstOrDefault(p => p.ProjectID == estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == item.ApartmentID).BuildingID).BuildingID).Location;
        }

        public string ClientName
        {
            get => estateEntities.Clients.FirstOrDefault(c => c.ClientID == item.ClientID)?.FirstName;
        }

        public string ClientSurname
        {
            get => estateEntities.Clients.FirstOrDefault(c => c.ClientID == item.ClientID)?.LastName;
        }

        public decimal SalePrice
        {
            get => item.SalePrice;
            set
            {
                item.SalePrice = value;
                OnPropertyChanged(() => SalePrice);
            }
        }

        public DateTime SaleDate
        {
            get => item.SaleDate;
            set
            {
                item.SaleDate = value;
                OnPropertyChanged(() => SaleDate);
            }
        }

        public string Status
        {
            get => item.Status;
            set
            {
                item.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        public RealyCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DeleteSaleModalViewModel(Sales sale)
            : base()
        {
            item = sale;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Sales.FirstOrDefault(s => s.SaleID == item.SaleID);
            if (existingItem != null)
            {
                estateEntities.Sales.Remove(existingItem);
                estateEntities.SaveChanges();
                SaveHistoryOfChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono sprzedaży do usunięcia.");
                errorModal.ShowDialog();
            }
        }

        private void ExecuteDelete(object parameter)
        {
            try
            {
                Delete();
                base.OnRequestClose();
            }
            catch (Exception ex)
            {
                var errorModal = new ErrorModalView($"Wystąpił błąd: {ex.Message}");
                errorModal.ShowDialog();
            }
        }

        private void CancelDelete(object parameter)
        {
            base.OnRequestClose();
        }
        #endregion
    }
}