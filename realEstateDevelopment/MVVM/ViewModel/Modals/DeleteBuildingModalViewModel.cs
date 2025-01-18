﻿using realEstateDevelopment.Core;
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
    public class DeleteBuildingModalViewModel : BaseDataDeleter<Buildings>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int BuildingID
        {
            get => item.BuildingID;
            set
            {
                item.BuildingID = value;
                OnPropertyChanged(() => BuildingID);
            }
        }

        public int ProjectID
        {
            get => item.ProjectID;
            set
            {
                item.ProjectID = value;
                OnPropertyChanged(() => ProjectID);
            }
        }

        public string BuildingName
        {
            get => item.BuildingName;
            set
            {
                item.BuildingName = value;
                OnPropertyChanged(() => BuildingName);
            }
        }

        public string BuildingNumber
        {
            get => item.BuildingNumber;
            set
            {
                item.BuildingNumber = value;
                OnPropertyChanged(() => BuildingNumber);
            }
        }

        public int Floors
        {
            get => item.Floors;
            set
            {
                item.Floors = value;
                OnPropertyChanged(() => Floors);
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

        public string Localization
        {
            get
            {
                // Jeśli chcesz wyświetlić lokalizację związanego projektu
                return estateEntities.Projects.FirstOrDefault(p => p.ProjectID == item.ProjectID)?.Location;
            }
        }
        #endregion


        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        #endregion 

        #region Constructor
        public DeleteBuildingModalViewModel(Buildings building)
            : base()
        {
            item = building;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);

        }
        #endregion




        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == item.BuildingID);
            if (existingItem != null)
            {
                // Usuń harmonogramy budynku
                var constructionSchedulesToRemove = estateEntities.ConstructionSchedule
                    .Where(c => c.BuildingId == existingItem.BuildingID)
                    .ToList();
                foreach (var constructionSchedule in constructionSchedulesToRemove)
                {
                    estateEntities.ConstructionSchedule.Remove(constructionSchedule);
                }

                // Przygotowanie mieszkań do usunięcia
                var apartmentsToRemove = estateEntities.Apartments
                    .Where(a => a.BuildingID == existingItem.BuildingID)
                    .ToList();

                foreach (var apartment in apartmentsToRemove)
                {
                    // Usuń sprzedaż powiązaną z mieszkaniem
                    var salesToRemove = estateEntities.Sales
                        .Where(s => s.ApartmentID == apartment.ApartmentID)
                        .ToList();
                    foreach (var sale in salesToRemove)
                    {
                        estateEntities.Sales.Remove(sale);
                    }

                    // Usuń zgłoszenia konserwacyjne powiązane z mieszkaniem
                    var maintenanceRequestsToRemove = estateEntities.MaintenanceRequests
                        .Where(m => m.ApartmentID == apartment.ApartmentID)
                        .ToList();
                    foreach (var maintenanceRequest in maintenanceRequestsToRemove)
                    {
                        estateEntities.MaintenanceRequests.Remove(maintenanceRequest);
                    }

                    // Usuń wynajem powiązany z mieszkaniem
                    var rentalsToRemove = estateEntities.Rental
                        .Where(r => r.ApartmentId == apartment.ApartmentID)
                        .ToList();
                    foreach (var rental in rentalsToRemove)
                    {
                        estateEntities.Rental.Remove(rental);
                    }

                    // Usuń mieszkanie
                    estateEntities.Apartments.Remove(apartment);
                }

                // Usuń budynek
                estateEntities.Buildings.Remove(existingItem);

                // Zapisz zmiany
                estateEntities.SaveChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono elementu do usunięcia.");
                errorModal.ShowDialog();
            }
        }


        public void ShowMessageBox(string message)
        {
            // Zamiast wyświetlać alert, otwórz modal.
            var deleteBuildingModal = new DeleteBuildingModalView(); // To powinien być Twój UserControl/Window
            deleteBuildingModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                deleteBuildingModal.Close();
            };

            deleteBuildingModal.ShowDialog(); // Jeśli to jest Window
        }

        private void ExecuteDelete(object parameter)
        {
            try
            {
                Delete(); // Wywołanie istniejącej metody Delete
                base.OnRequestClose();
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                var errorModal = new ErrorModalView($"Wystąpił błąd: {ex.Message}");
                errorModal.ShowDialog();
            }
        }
        #endregion
    }
}
