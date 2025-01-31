using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using System.Collections.Generic;
using System;
using realEstateDevelopment.MVVM.View.Modals;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewSupplierViewModel : BaseDatabaseAdder<Suppliers>
    {
        #region Properties
        public string CompanyName
        {
            get
            {
                return item.CompanyName;
            }
            set
            {
                item.CompanyName = value;
                OnPropertyChanged(() => CompanyName);
            }
        }
        public string Contact
        {
            get
            {
                return item.Contact;
            }
            set
            {
                item.Contact = value;
                OnPropertyChanged(() => Contact);
            }
        }

        public string Address
        {
            get
            {
                return item.Address;
            }
            set
            {
                item.Address = value;
                OnPropertyChanged(() => Address);
            }
        }
      

        #endregion

        #region Constructor
        public AddNewSupplierViewModel()
        {
            item = new Suppliers();
        }
        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                errors.Add("Nazwa firmy jest wymagana.");
                isDataCorrect = false;
            }
            if (string.IsNullOrWhiteSpace(Contact))
            {
                errors.Add("Kontakt jest wymagany.");
                isDataCorrect = false;
            }
            if (string.IsNullOrWhiteSpace(Address))
            {
                errors.Add("Adres jest wymagany.");
                isDataCorrect = false;
            }
            potentialErrors = string.Join(Environment.NewLine, errors);
        }
        public override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(CompanyName):
                    return string.IsNullOrWhiteSpace(CompanyName) ? "Nazwa firmy jest wymagana." : string.Empty;

                case nameof(Contact):
                    return string.IsNullOrWhiteSpace(Contact) ? "Kontakt jest wymagany." : string.Empty;

                case nameof(Address):
                    return string.IsNullOrWhiteSpace(Address) ? "Adres jest wymagany." : string.Empty;

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
                estateEntities.Suppliers.Add(item);
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
