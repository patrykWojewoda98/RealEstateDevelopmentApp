using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewPurchaseViewModel : BaseDatabaseAdder<Purchases>
    {
        #region Properites
        private string _selectedType;

        public ObservableCollection<string> AllTypes { get; set; }
       

        public string SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                if (_selectedType != null)
                {
                    item.TypeOfPurchase = _selectedType;
                }
                OnPropertyChanged(() => _selectedType);
            }
        }

        public DateTime PurchaseDate
        {
            get => item.PurchaseDate;
            set
            {
                item.PurchaseDate = DateTime.Now;
                OnPropertyChanged(() => PurchaseDate);
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
        #endregion

        #region Constructor
        public AddNewPurchaseViewModel()
            :base()
        {
            item = new Purchases();

            AllTypes = new ObservableCollection<string>();
            AllTypes.Add("Grunty");
            AllTypes.Add("Materiały");
            SelectedType = AllTypes.FirstOrDefault();
            PurchaseDate = DateTime.Now;
        }
        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (PurchaseDate == null)
            {
                errors.Add("Nie wybrano daty zakupu!");
                isDataCorrect = false;
            }

            if (PurchaseDate <= DateTime.Now.AddYears(-1))
            {
                errors.Add("Nie można zaksiegować zakupu zrobionego ponad rok wstecz.");
                isDataCorrect = false;
            }

            if (Amount <=0)
            {
                errors.Add("Kwota zakupu nie moze być mniejsza bądz równa 0.");
                isDataCorrect = false;
            }

            potentialErrors = string.Join(Environment.NewLine, errors);
        }
        public override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Amount):
                    return Amount <= 0 ? "Kwota zakupu nie może być mniejsza bądź równa 0." : string.Empty;

                default:
                    return string.Empty;
            }
        }


        #endregion

        #region Helpers

        public override void Save()
        {
            Validate();
            if (isDataCorrect)
            {
                SaveHistoryOfChanges();
                estateEntities.Purchases.Add(item);
                estateEntities.SaveChanges();
            }
            else
            {
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }
        #endregion
    }
}

