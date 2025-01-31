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
    public class DeleteExpenseModalViewModel : BaseDataDeleter<Expenses>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int Id
        {
            get => item.ExpenseID;
            set
            {
                item.ExpenseID = value;
                OnPropertyChanged(() => Id);
            }
        }

        public string ProjectName
        {
            get => estateEntities.Projects.FirstOrDefault(p => p.ProjectID == item.ProjectID)?.ProjectName;
        }

        public string Address
        {
            get => estateEntities.Projects.FirstOrDefault(p => p.ProjectID == item.ProjectID)?.Location;
        }

        public string ExpenseType
        {
            get => item.ExpenseType;
            set
            {
                item.ExpenseType = value;
                OnPropertyChanged(() => ExpenseType);
            }
        }

        public decimal ExpenseAmount
        {
            get => item.Amount;
            set
            {
                item.Amount = value;
                OnPropertyChanged(() => ExpenseAmount);
            }
        }

        public DateTime ExpenseDate
        {
            get => item.ExpenseDate;
            set
            {
                item.ExpenseDate = value;
                OnPropertyChanged(() => ExpenseDate);
            }
        }

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        public RealyCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DeleteExpenseModalViewModel(Expenses expense)
            : base()
        {
            item = expense;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Expenses.FirstOrDefault(e => e.ExpenseID == item.ExpenseID);
            if (existingItem != null)
            {
                estateEntities.Expenses.Remove(existingItem);
                estateEntities.SaveChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono wydatku do usunięcia.");
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
