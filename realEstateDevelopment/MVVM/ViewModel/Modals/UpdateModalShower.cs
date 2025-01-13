using GalaSoft.MvvmLight.Messaging;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Linq;

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
            else
            {
                Console.WriteLine("obslużyć bład");
            }
        }
    }
}
