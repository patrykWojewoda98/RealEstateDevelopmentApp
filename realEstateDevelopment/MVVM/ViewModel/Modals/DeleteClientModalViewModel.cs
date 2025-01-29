using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteClientModalViewModel : BaseDataDeleter<Clients>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int Id
        {
            get => item.ClientID;
            set
            {
                item.ClientID = value;
                OnPropertyChanged(() => Id);
            }
        }

        public string Name
        {
            get => item.FirstName;
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => Name);
            }
        }

        public string Surname
        {
            get => item.LastName;
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => Surname);
            }
        }

        public string Pesel
        {
            get => item.Pesel;
            set
            {
                item.Pesel = value;
                OnPropertyChanged(() => Pesel);
            }
        }

        public int IdCardNumber
        {
            get => item.IdCardNumber;
            set
            {
                item.IdCardNumber = value;
                OnPropertyChanged(() => IdCardNumber);
            }
        }

        public string IdCardSeries
        {
            get => item.IdCardSeries;
            set
            {
                item.IdCardSeries = value;
                OnPropertyChanged(() => IdCardSeries);
            }
        }

        public string PhoneNumber
        {
            get => item.PhoneNumber;
            set
            {
                item.PhoneNumber = value;
                OnPropertyChanged(() => PhoneNumber);
            }
        }

        public string Email
        {
            get => item.Email;
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
            }
        }

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        #endregion

        #region Constructor
        public DeleteClientModalViewModel(Clients client)
            : base()
        {
            item = client;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Clients.FirstOrDefault(c => c.ClientID == item.ClientID);
            if (existingItem != null)
            {
                // Usuń powiązane rezerwacje klienta
                var reservationsToRemove = estateEntities.Reservations
                    .Where(r => r.ClientID == existingItem.ClientID)
                    .ToList();
                foreach (var reservation in reservationsToRemove)
                {
                    estateEntities.Reservations.Remove(reservation);
                }

                // Usuń powiązane sprzedaże klienta
                var salesToRemove = estateEntities.Sales
                    .Where(s => s.ClientID == existingItem.ClientID)
                    .ToList();
                foreach (var sale in salesToRemove)
                {
                    estateEntities.Sales.Remove(sale);
                }

                // Usuń powiązane wynajmy klienta
                var rentalsToRemove = estateEntities.Rental
                    .Where(r => r.ClientId == existingItem.ClientID)
                    .ToList();
                foreach (var rental in rentalsToRemove)
                {
                    estateEntities.Rental.Remove(rental);
                }

                // Usuń powiązane płatności klienta
                var paymentsToRemove = estateEntities.Payments
                    .Where(p => p.ClientID == existingItem.ClientID)
                    .ToList();
                foreach (var payment in paymentsToRemove)
                {
                    estateEntities.Payments.Remove(payment);
                }

                // Usuń powiązane zgłoszenia konserwacyjne klienta
                var maintenanceRequestsToRemove = estateEntities.MaintenanceRequests
                    .Where(m => m.ClientID == existingItem.ClientID)
                    .ToList();
                foreach (var maintenanceRequest in maintenanceRequestsToRemove)
                {
                    estateEntities.MaintenanceRequests.Remove(maintenanceRequest);
                }

                // Usuń klienta
                estateEntities.Clients.Remove(existingItem);
                estateEntities.SaveChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono klienta do usunięcia.");
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
        #endregion
    }
}
