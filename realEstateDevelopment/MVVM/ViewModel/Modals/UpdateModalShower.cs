using GalaSoft.MvvmLight.Messaging;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Linq;
using System.Windows.Data;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateModalShower
    {
        // Pole przechowujące instancję Singletona
        private static UpdateModalShower _instance;

        protected readonly RealEstateEntities realEstateEntities;

        // Publiczna właściwość umożliwiająca dostęp do instancji
        public static UpdateModalShower Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UpdateModalShower();
                }
                return _instance;
            }
        }

        // Prywatny konstruktor, aby uniemożliwić bezpośrednie tworzenie instancji
        private UpdateModalShower()
        {
            // Rejestracja generycznej wiadomości
            Messenger.Default.Register<UpdateMessage>(this, Open);
            realEstateEntities = new RealEstateEntities();
        }

        // Metoda obsługująca otrzymane wiadomości
        private void Open(UpdateMessage updateMessage)
        {
            // Sprawdzenie wiadomości
            if (updateMessage.Message == "ApartmentsViewModelUpdate")
            {
                // Rzutowanie danych, jeśli są potrzebne
                var data = updateMessage.Data;

                // Otwórz modal, przekazując dane
                var apartment = realEstateEntities.Apartments.FirstOrDefault(a => a.ApartmentID == data);

                if (apartment != null)
                {
                    var updateModal = new UpdateApartmentModalViewModel(apartment);
                    updateModal.ShowMessageBox($"Update for Apartment: {apartment.ApartmentNumber}");
                }
                else
                {
                    Console.WriteLine("No apartment found with the provided ID.");
                }
            } else if (updateMessage.Message == "BuildingsViewModelUpdate")
            {
                var building = realEstateEntities.Buildings.FirstOrDefault(b => b.BuildingID == updateMessage.Data);
                if (building != null)
                {
                    // dokończyć
                    var updateModal = new UpdateBuildingModalViewModel(building);
                    updateModal.ShowMessageBox($"Update for Apartment: {building.BuildingNumber}");
                }

            }
            else if (updateMessage.Message == "ConstructionScheduleViewModelUpdate")
            {
                var constructionSchedule = realEstateEntities.ConstructionSchedule.FirstOrDefault(c => c.ScheduleID == updateMessage.Data);
                if (constructionSchedule != null)
                {
                    // dokończyć
                    var updateModal = new UpdateBuildingConstructionScheduleModalViewModel(constructionSchedule);
                    updateModal.ShowMessageBox($"Update for Apartment: {constructionSchedule.ScheduleID}");
                }

            }
            else if (updateMessage.Message == "ClientViewModelUpdate")
            {
                var client = realEstateEntities.Clients.FirstOrDefault(c => c.ClientID == updateMessage.Data);
                if (client != null)
                {
                    // dokończyć
                    var updateModal = new UpdateClientModalViewModel(client);
                    updateModal.ShowMessageBox($"Update for Apartment: {client.ClientID}");
                    
                }

            }
            else
            {
                Console.WriteLine("obslużyć bład");
            }
        }
    }
}
