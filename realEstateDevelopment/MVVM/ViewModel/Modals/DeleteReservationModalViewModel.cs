using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteReservationModalViewModel : BaseDataDeleter<Reservations>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int Id
        {
            get => item.ReservationID;
            set
            {
                item.ReservationID = value;
                OnPropertyChanged(() => Id);
            }
        }

        public string ApartmentNumber
        {
            get => estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == item.ApartmentID)?.ApartmentNumber;
        }

        public string BuildingNumber
        {
            get => estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == estateEntities.Apartments.FirstOrDefault(a=>a.ApartmentID == item.ApartmentID).BuildingID).BuildingNumber;
        }

        public string Address
        {
            get => estateEntities.Projects.FirstOrDefault(p => p.ProjectID == 
                   estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == 
                   estateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == item.ApartmentID).BuildingID).ProjectID).Location; 
        }

        public DateTime ReservationDate
        {
            get => item.ReservationDate;
            set
            {
                item.ReservationDate = value;
                OnPropertyChanged(() => ReservationDate);
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

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        public RealyCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DeleteReservationModalViewModel(Reservations reservation)
            : base()
        {
            item = reservation;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Reservations.FirstOrDefault(r => r.ReservationID == item.ReservationID);
            if (existingItem != null)
            {
                estateEntities.Reservations.Remove(existingItem);
                estateEntities.SaveChanges();
                SaveHistoryOfChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono rezerwacji do usunięcia.");
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
