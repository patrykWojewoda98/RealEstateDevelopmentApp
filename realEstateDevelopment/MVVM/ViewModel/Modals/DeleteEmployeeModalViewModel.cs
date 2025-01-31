using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteEmployeeModalViewModel : BaseDataDeleter<Employees>
    {
        #region Properties
        public int EmployeeId
        {
            get => item.EmployeeID;
            set
            {
                item.EmployeeID = value;
                OnPropertyChanged(() => EmployeeId);
            }
        }

        public string FirstName
        {
            get => item.FirstName;
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get => item.LastName;
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => LastName);
            }
        }

        public string Position
        {
            get => item.Position;
            set
            {
                item.Position = value;
                OnPropertyChanged(() => Position);
            }
        }

        public string Department
        {
            get => item.Department;
            set
            {
                item.Department = value;
                OnPropertyChanged(() => Department);
            }
        }

        public decimal Salary
        {
            get => (decimal)item.Salary;
            set
            {
                item.Salary = value;
                OnPropertyChanged(() => Salary);
            }
        }

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        public RealyCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DeleteEmployeeModalViewModel(Employees employee)
            : base()
        {
            item = employee;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Employees.FirstOrDefault(e => e.EmployeeID == item.EmployeeID);
            if (existingItem != null)
            {
                // Usuń powiązania z projektami
                var employeeProjectsToRemove = estateEntities.EmployeeProjects
                    .Where(ep => ep.EmployeeID == existingItem.EmployeeID)
                    .ToList();
                foreach (var ep in employeeProjectsToRemove)
                {
                    estateEntities.EmployeeProjects.Remove(ep);
                }

                // Usuń historię zmian związanych z pracownikiem
                var historyToRemove = estateEntities.HistoryOfChanges
                    .Where(h => h.EmployeeId == existingItem.EmployeeID)
                    .ToList();
                foreach (var history in historyToRemove)
                {
                    estateEntities.HistoryOfChanges.Remove(history);
                }

                // Usuń pracownika
                estateEntities.Employees.Remove(existingItem);
                estateEntities.SaveChanges();
                SaveHistoryOfChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono pracownika do usunięcia.");
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
