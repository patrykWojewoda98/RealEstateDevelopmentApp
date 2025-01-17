using GalaSoft.MvvmLight.Messaging;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DelateModalShower
    {
        private static DelateModalShower _instance;

        protected readonly RealEstateEntities realEstateEntities;

        public static DelateModalShower Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DelateModalShower();
                }
                return _instance;
            }
        }

        // Prywatny konstruktor, aby uniemożliwić bezpośrednie tworzenie instancji
        private DelateModalShower()
        {
            // Rejestracja generycznej wiadomości
            Messenger.Default.Register<DeleteMessage>(this, Open);
            realEstateEntities = new RealEstateEntities();
        }


        private void Open(DeleteMessage updateMessage)
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
                    var deleteModal = new DeleteApartmentModalViewModel(apartment);
                    deleteModal.ShowMessageBox($"Update for Apartment: {apartment.ApartmentNumber}");
                }
                else
                {
                    Console.WriteLine("No apartment found with the provided ID.");
                }
            }
            else if (updateMessage.Message == "BuildingsViewModelUpdate")
            {
                var building = realEstateEntities.Buildings.FirstOrDefault(b => b.BuildingID == updateMessage.Data);
                if (building != null)
                {
                    // dokończyć
                    var updateModal = new UpdateBuildingModalViewModel(building);
                    updateModal.ShowMessageBox($"Update for Building: {building.BuildingNumber}");
                }

            }
            else if (updateMessage.Message == "ConstructionScheduleViewModelUpdate")
            {
                var constructionSchedule = realEstateEntities.ConstructionSchedule.FirstOrDefault(c => c.ScheduleID == updateMessage.Data);
                if (constructionSchedule != null)
                {
                    // dokończyć
                    var updateModal = new UpdateBuildingConstructionScheduleModalViewModel(constructionSchedule);
                    updateModal.ShowMessageBox($"Update for Construction Schedule: {constructionSchedule.ScheduleID}");
                }

            }
            else if (updateMessage.Message == "ClientViewModelUpdate")
            {
                var client = realEstateEntities.Clients.FirstOrDefault(c => c.ClientID == updateMessage.Data);
                if (client != null)
                {
                    // dokończyć
                    var updateModal = new UpdateClientModalViewModel(client);
                    updateModal.ShowMessageBox($"Update for Client: {client.ClientID}");
                }
            }
            else if (updateMessage.Message == "ProjectsViewModelUpdate")
            {
                var project = realEstateEntities.Projects.FirstOrDefault(p => p.ProjectID == updateMessage.Data);
                if (project != null)
                {
                    var updateModal = new UpdateProjectModalViewModel(project);
                    updateModal.ShowMessageBox($"Update for Project: {project.ProjectID}");

                }
            }
            else if (updateMessage.Message == "MaterialsViewModelUpdate")
            {
                var material = realEstateEntities.Materials.FirstOrDefault(m => m.MaterialID == updateMessage.Data);
                if (material != null)
                {
                    var updateModal = new UpdateMaterialModalViewModel(material);
                    updateModal.ShowMessageBox($"Update for Project: {material.MaterialID}");

                }
            }
            else if (updateMessage.Message == "ReservationsViewModelUpdate")
            {
                var reservation = realEstateEntities.Reservations.FirstOrDefault(r => r.ReservationID == updateMessage.Data);
                if (reservation != null)
                {
                    var updateModal = new UpdateReservationModalViewModel(reservation);
                    updateModal.ShowMessageBox($"Update for Project: {reservation.ReservationID}");

                }
            }
            else if (updateMessage.Message == "ExpenceUpdate")
            {
                var expense = realEstateEntities.Expenses.FirstOrDefault(e => e.ExpenseID == updateMessage.Data);
                if (expense != null)
                {
                    var updateModal = new UpdateExpensesModalViewModel(expense);
                    updateModal.ShowMessageBox($"Update for Project: {expense.ExpenseID}");

                }
            }
            else if (updateMessage.Message == "RevenueUpdate")
            {
                var revenue = realEstateEntities.Revenues.FirstOrDefault(r => r.RevenueID == updateMessage.Data);
                if (revenue != null)
                {
                    var updateModal = new UpdateRevenueModalViewModel(revenue);
                    updateModal.ShowMessageBox($"Update for Project: {revenue.RevenueID}");

                }
            }
            else if (updateMessage.Message == "MaintenanceRequestsViewModelUpdate")
            {
                var maintenanceRequest = realEstateEntities.MaintenanceRequests.FirstOrDefault(m => m.RequestID == updateMessage.Data);
                if (maintenanceRequest != null)
                {
                    var updateModal = new UpdateMaintenanceRequestModalViewModel(maintenanceRequest);
                    updateModal.ShowMessageBox($"Update for Project: {maintenanceRequest.RequestID}");

                }
            }
            else if (updateMessage.Message == "EmployeesViewModelUpdate")
            {
                var updateEmployees = realEstateEntities.Employees.FirstOrDefault(e => e.EmployeeID == updateMessage.Data);
                if (updateEmployees != null)
                {
                    var updateModal = new UpdateEmployeesModalViewModel(updateEmployees);
                    updateModal.ShowMessageBox($"Update for Project: {updateEmployees.EmployeeID}");

                }
            }
            else if (updateMessage.Message == "SuppliersViewModelUpdate")
            {
                var updateSupplier = realEstateEntities.Suppliers.FirstOrDefault(s => s.SupplierID == updateMessage.Data);
                if (updateSupplier != null)
                {
                    var updateModal = new UpdateSupplierModalViewModel(updateSupplier);
                    updateModal.ShowMessageBox($"Update for Project: {updateSupplier.SupplierID}");

                }
            }
            else if (updateMessage.Message == "SalesViewModelUpdate")
            {
                var updateSale = realEstateEntities.Sales.FirstOrDefault(s => s.SaleID == updateMessage.Data);
                if (updateSale != null)
                {
                    var updateModal = new UpdateSalesModalViewModel(updateSale);
                    updateModal.ShowMessageBox($"Update for Project: {updateSale.SaleID}");

                }
            }
            else if (updateMessage.Message == "PurchasesViewModelUpdate")
            {
                var updatePurchase = realEstateEntities.Purchases.FirstOrDefault(p => p.PurchaseID == updateMessage.Data);
                if (updatePurchase != null)
                {
                    var updateModal = new UpdatePurchaseModalViewModel(updatePurchase);
                    updateModal.ShowMessageBox($"Update for Project: {updatePurchase.PurchaseID}");

                }
            }

            else
            {
                Console.WriteLine("obslużyć bład");

            }
        }
    }
}
