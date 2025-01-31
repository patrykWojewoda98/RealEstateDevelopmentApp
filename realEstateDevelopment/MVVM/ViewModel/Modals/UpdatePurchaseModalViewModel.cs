using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdatePurchaseModalViewModel : BaseDataUpdater<Purchases>
    {
        #region Properties


        

        public int PurchaseID
        {
            get => item.PurchaseID;
            set
            {
                item.PurchaseID = value;
                OnPropertyChanged(() => PurchaseID);
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

        public string TypeOfPurchase
        {
            get => item.TypeOfPurchase;
            set
            {
                item.TypeOfPurchase = value;
                OnPropertyChanged(() => TypeOfPurchase);
            }
        }



        #endregion

        #region Constructor

        public UpdatePurchaseModalViewModel(Purchases purchase)
            : base()
        {
            item = purchase;
            
        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(TypeOfPurchase))
            {
                errors.Add("Typ zakupu jest wymagany.");
                isDataCorrect = false;
            }

            if (Amount <= 0)
            {
                errors.Add("Kwota musi być większa od 0.");
                isDataCorrect = false;
            }

            if (PurchaseDate == default(DateTime))
            {
                errors.Add("Data zakupu jest wymagana.");
                isDataCorrect = false;
            }

            potentialErrors = string.Join(Environment.NewLine, errors);
        }


        #endregion

        #region Helpers

        public override void Update()
        {
            Validate();

            if (isDataCorrect)
            {
                var existingItem = estateEntities.Purchases.FirstOrDefault(p => p.PurchaseID == item.PurchaseID);

                if (existingItem != null)
                {
                    existingItem.TypeOfPurchase = item.TypeOfPurchase;
                    existingItem.Amount = item.Amount;
                    existingItem.PurchaseDate = item.PurchaseDate;

                    estateEntities.SaveChanges();
                    SaveHistoryOfChanges();
                }
                else
                {
                    var errorModal = new ErrorModalView("Nie znaleziono zakupu do aktualizacji.");
                    errorModal.ShowDialog();
                }
            }
            else
            {
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }


        public void ShowMessageBox(string message)
        {
            var updatePurchaseModal = new UpdatePurchaseModalView();
            updatePurchaseModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updatePurchaseModal.Close();
            };

            updatePurchaseModal.ShowDialog();
        }

        

        #endregion
    }
}
