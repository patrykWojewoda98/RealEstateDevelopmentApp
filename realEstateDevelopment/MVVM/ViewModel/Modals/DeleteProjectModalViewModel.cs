using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteProjectModalViewModel : BaseDataDeleter<Projects>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int ProjectId
        {
            get => item.ProjectID;
            set
            {
                item.ProjectID = value;
                OnPropertyChanged(() => ProjectId);
            }
        }

        public string ProjectName
        {
            get => item.ProjectName;
            set
            {
                item.ProjectName = value;
                OnPropertyChanged(() => ProjectName);
            }
        }

        public string ProjectLocalization
        {
            get => item.Location;
            set
            {
                item.Location = value;
                OnPropertyChanged(() => ProjectLocalization);
            }
        }

        public DateTime StartDate
        {
            get => (DateTime)item.StartDate;
            set
            {
                item.StartDate = value;
                OnPropertyChanged(() => StartDate);
            }
        }

        public DateTime EndDate
        {
            get => (DateTime)item.EndDate;
            set
            {
                item.EndDate = value;
                OnPropertyChanged(() => EndDate);
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
        public DeleteProjectModalViewModel(Projects project)
            : base()
        {
            item = project;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Projects.FirstOrDefault(p => p.ProjectID == item.ProjectID);
            if (existingItem != null)
            {
                // Usuń powiązane apartamenty oraz ich zależności
                var apartmentsToRemove = estateEntities.Apartments
                    .Where(a => estateEntities.Buildings.Any(b => b.ProjectID == existingItem.ProjectID && b.BuildingID == a.BuildingID))
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

                    estateEntities.Apartments.Remove(apartment);
                }

                // Usuń powiązane budynki
                var buildingsToRemove = estateEntities.Buildings
                    .Where(b => b.ProjectID == existingItem.ProjectID)
                    .ToList();
                foreach (var building in buildingsToRemove)
                {
                    estateEntities.Buildings.Remove(building);
                }

                // Usuń powiązane wydatki
                var expensesToRemove = estateEntities.Expenses
                    .Where(e => e.ProjectID == existingItem.ProjectID)
                    .ToList();
                foreach (var expense in expensesToRemove)
                {
                    estateEntities.Expenses.Remove(expense);
                }

                // Usuń harmonogram budowy
                var schedulesToRemove = estateEntities.ConstructionSchedule
                    .Where(s => s.ProjectID == existingItem.ProjectID)
                    .ToList();
                foreach (var schedule in schedulesToRemove)
                {
                    estateEntities.ConstructionSchedule.Remove(schedule);
                }

                estateEntities.Projects.Remove(existingItem);
                estateEntities.SaveChanges();
                SaveHistoryOfChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono projektu do usunięcia.");
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
