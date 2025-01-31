using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteMaintenanceRequestModalViewModel : BaseDataDeleter<MaintenanceRequests>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int Id
        {
            get => item.RequestID;
            set
            {
                item.RequestID = value;
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
            get => estateEntities.Projects.FirstOrDefault(p => p.ProjectID == 
                   estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == 
                   estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == item.ApartmentID).BuildingID).ProjectID).Location;
        }

        public string Description
        {
            get => item.Description;
            set
            {
                item.Description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public DateTime RequestDate
        {
            get => (DateTime)item.RequestDate;
            set
            {
                item.RequestDate = value;
                OnPropertyChanged(() => RequestDate);
            }
        }

        public string ClientName
        {
            get => estateEntities.Clients.FirstOrDefault(c => c.ClientID == item.ClientID)?.FirstName;
        }

        public string ClientSurname
        {
            get => estateEntities.Clients.FirstOrDefault(c => c.ClientID == item.ClientID)?.LastName;
        }

        public string ClientPhoneNumber
        {
            get => estateEntities.Clients.FirstOrDefault(c => c.ClientID == item.ClientID)?.PhoneNumber;
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
        public DeleteMaintenanceRequestModalViewModel(MaintenanceRequests request)
            : base()
        {
            item = request;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.MaintenanceRequests.FirstOrDefault(r => r.RequestID == item.RequestID);
            if (existingItem != null)
            {
                estateEntities.MaintenanceRequests.Remove(existingItem);
                estateEntities.SaveChanges();
                SaveHistoryOfChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono zgłoszenia do usunięcia.");
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
