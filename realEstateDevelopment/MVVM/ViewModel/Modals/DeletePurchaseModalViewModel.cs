using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeletePurchaseModalViewModel : BaseDataDeleter<Purchases>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int PurchaseID
        {
            get => item.PurchaseID;
            set
            {
                item.PurchaseID = value;
                OnPropertyChanged(() => PurchaseID);
            }
        }

        public string TypeOfPurchase
        {
            get => item.TypeOfPurchase;
            set
            {
                item.TypeOfPurchase = value;
                OnPropertyChanged(() => TypeOfPurchase);
            }
        }

        public decimal Amount
        {
            get => item.Amount;
            set
            {
                item.Amount = value;
                OnPropertyChanged(() => Amount);
            }
        }

        public DateTime PurchaseDate
        {
            get => item.PurchaseDate;
            set
            {
                item.PurchaseDate = value;
                OnPropertyChanged(() => PurchaseDate);
            }
        }

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        public RealyCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DeletePurchaseModalViewModel(Purchases purchase)
            : base()
        {
            item = purchase;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Purchases.FirstOrDefault(p => p.PurchaseID == item.PurchaseID);
            if (existingItem != null)
            {
                estateEntities.Purchases.Remove(existingItem);
                estateEntities.SaveChanges();
                SaveHistoryOfChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono zakupu do usunięcia.");
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
