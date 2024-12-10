using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System.Collections.Generic;
using System;
using realEstateDevelopment.Helper;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewMaterialViewModel:BaseDatabaseAdder<Materials>
    {
        #region Propeties
        public int MaterialID
        {
            get
            {
                return item.MaterialID;
            }
            set
            {
                item.MaterialID = value;
                OnPropertyChanged(() => MaterialID);
            }
        }

        public int SupplierID
        {
            get
            {
                return item.SupplierID;
            }
            set
            {
                item.SupplierID = value;
                OnPropertyChanged(() => SupplierID);
            }
        }

        public string MaterialName { 
            get
            {
                return item.MaterialName;
            }
            set
            {
                item.MaterialName = value;
                OnPropertyChanged(() => MaterialName);
            }
        }
        public string Type
        {
            get
            {
                return item.Type;
            }
            set
            {
                item.Type = value;
                OnPropertyChanged(() => Type);
            }
        }


        public decimal UnitPrice
        {
            get
            {
                return item.UnitPrice;
            }
            set
            {
                item.UnitPrice = value;
                OnPropertyChanged(() => UnitPrice);
            }
        }
        

        #endregion
        #region Constructor
        public AddNewMaterialViewModel()
            : base()
        {
            item = new Materials();
        }


        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();
            var db = new RealEstateEntities();

            if(db.Materials.Any(m=>m.MaterialID == MaterialID)) {
                errors.Add("Materiał o podanym Numerze ID już istnieje.");
                isDataCorrect = false;
            }

             if (MaterialID <= 0)
            {
                errors.Add("Id Materiału nie może być mniejszy lub równy 0");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(MaterialName))
            {
                errors.Add("Nazwa materiału jest wymagana.");
                isDataCorrect = false;
            }

            if (SupplierID<=0)
            {
                errors.Add("Id dostawcy nie może być ujemne lub być \"0\"");
                isDataCorrect = false;
            }

            if (UnitPrice < 0)
            {
                errors.Add("Kwota musi być większa od 0.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Type))
            {
                errors.Add("Typ materiału jest wymagany.");
                isDataCorrect = false;
            }
            potentialErrors = string.Join(Environment.NewLine, errors);
        }
        #endregion


        #region Helpers

        public override void Save()
        {
            Validate();
            if (isDataCorrect)
            {
                estateEntities.Materials.Add(item);
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
