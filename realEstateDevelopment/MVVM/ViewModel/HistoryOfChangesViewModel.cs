using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class HistoryOfChangesViewModel : LoadAllViewModel<HistoryOfChangesEntityForView>
    {
        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        #region Helpers
        public override async Task LoadAsync()
        {
            try
            {
                var query = from h in realEstateEntities.HistoryOfChanges
                            join e in realEstateEntities.Employees on h.EmployeeId equals e.EmployeeID
                            select new HistoryOfChangesEntityForView
                            {
                                Id = h.IdOfChange,
                                EmployeeName = e.FirstName,
                                EmployeeSurname = e.LastName,
                                Operation = h.Operation,
                                DateAndTimeOfChange = h.DateAndTimeOfChange
                            };

                var result = await query.ToListAsync();

                // Zamiana operacji na czytelne nazwy
                foreach (var item in result)
                {
                    item.Operation = MapOperationName(item.Operation);
                }

                List = new ObservableCollection<HistoryOfChangesEntityForView>(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania danych: {ex.Message}");
            }
        }

        private string MapOperationName(string operation)
        {
            if (string.IsNullOrEmpty(operation))
                return "Nieznana operacja";

            switch (operation)
            {
                case "AddNewApartmentViewModel": return "Dodanie nowego apartamentu";
                case "AddNewBuildingViewModel": return "Dodanie nowego budynku";
                case "AddNewClientViewModel": return "Dodanie nowego klienta";
                case "AddNewEmployeeViewModel": return "Dodanie nowego pracownika";
                case "AddNewExpenseViewModel": return "Dodanie nowego wydatku";
                case "AddNewMaintenanceRequestsViewModel": return "Dodanie nowego zgłoszenia serwisowego";
                case "AddNewMaterialViewModel": return "Dodanie nowego materiału";
                case "AddNewProjectViewModel": return "Dodanie nowego projektu";
                case "AddNewPurchaseViewModel": return "Dodanie nowego zakupu";
                case "AddNewReservationsViewModel": return "Dodanie nowej rezerwacji";
                case "AddNewRevenueViewModel": return "Dodanie nowego przychodu";
                case "AddNewSaleViewModel": return "Dodanie nowej sprzedaży";
                case "AddNewSupplierViewModel": return "Dodanie nowego dostawcy";

                case "UpdateApartmentModalViewModel": return "Aktualizacja apartamentu";
                case "UpdateBuildingConstructionScheduleModalViewModel": return "Aktualizacja harmonogramu budowy";
                case "UpdateBuildingModalViewModel": return "Aktualizacja budynku";
                case "UpdateClientModalViewModel": return "Aktualizacja klienta";
                case "UpdateEmployeesModalViewModel": return "Aktualizacja pracownika";
                case "UpdateExpensesModalViewModel": return "Aktualizacja wydatku";
                case "UpdateMaintenanceRequestModalViewModel": return "Aktualizacja zgłoszenia serwisowego";
                case "UpdateMaterialModalViewModel": return "Aktualizacja materiału";
                case "UpdateProjectModalViewModel": return "Aktualizacja projektu";
                case "UpdatePurchaseModalViewModel": return "Aktualizacja zakupu";
                case "UpdateReservationModalViewModel": return "Aktualizacja rezerwacji";
                case "UpdateRevenueModalViewModel": return "Aktualizacja przychodu";
                case "UpdateSaleModalViewModel": return "Aktualizacja sprzedaży";

                case "DeleteApartmentModalViewModel": return "Usunięcie apartamentu";
                case "DeleteBuildingModalViewModel": return "Usunięcie budynku";
                case "DeleteClientModalViewModel": return "Usunięcie klienta";
                case "DeleteConstructionScheduleModalViewModel": return "Usunięcie harmonogramu budowy";
                case "DeleteEmployeeModalViewModel": return "Usunięcie pracownika";
                case "DeleteExpenseModalViewModel": return "Usunięcie wydatku";
                case "DeleteMaintenanceRequestModalViewModel": return "Usunięcie zgłoszenia serwisowego";
                case "DeleteMaterialModalViewModel": return "Usunięcie materiału";
                case "DeleteProjectModalViewModel": return "Usunięcie projektu";
                case "DeletePurchaseModalViewModel": return "Usunięcie zakupu";
                case "DeleteReservationModalViewModel": return "Usunięcie rezerwacji";
                case "DeleteRevenueModalViewModel": return "Usunięcie przychodu";
                case "DeleteSaleModalViewModel": return "Usunięcie sprzedaży";
                case "DeleteSupplierModalViewModel": return "Usunięcie dostawcy";

                default: return operation; // Jeśli nie ma dopasowania, zwróć oryginalną nazwę
            }
        }


        #endregion
    }
}
