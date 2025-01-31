using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteRevenueModalViewModel : BaseDataDeleter<Revenues>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int Id
        {
            get => item.RevenueID;
            set
            {
                item.RevenueID = value;
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

        public string RevenueType
        {
            get => item.RevenueType;
            set
            {
                item.RevenueType = value;
                OnPropertyChanged(() => RevenueType);
            }
        }

        public decimal RevenueAmount
        {
            get => item.Amount;
            set
            {
                item.Amount = value;
                OnPropertyChanged(() => RevenueAmount);
            }
        }

        public DateTime RevenueDate
        {
            get => item.RevenueDate;
            set
            {
                item.RevenueDate = value;
                OnPropertyChanged(() => RevenueDate);
            }
        }

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        public RealyCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DeleteRevenueModalViewModel(Revenues revenue)
            : base()
        {
            item = revenue;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Revenues.FirstOrDefault(r => r.RevenueID == item.RevenueID);
            if (existingItem != null)
            {
                estateEntities.Revenues.Remove(existingItem);
                estateEntities.SaveChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono przychodu do usunięcia.");
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
