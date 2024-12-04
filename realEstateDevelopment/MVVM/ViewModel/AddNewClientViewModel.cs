using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using System.Collections.Generic;
using System;
using realEstateDevelopment.MVVM.View.Modals;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewClientViewModel : BaseDatabaseAdder<Clients>
    {
        #region Properties
        public string FristName
        {
            get
            {
                return item.FirstName;
            }
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => FristName);
            }
        }

        public string LastName
        {
            get
            {
                return item.LastName;
            }
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => LastName);
            }
        }

        public string PhoneNumber
        {
            get
            {
                return item.PhoneNumber;
            }
            set
            {
                item.PhoneNumber = value;
                OnPropertyChanged(() => PhoneNumber);
            }
        }

        public string Email
        {
            get
            {
                return item?.Email;
            }
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
            }
        }


        #endregion

        #region Constructor
        public AddNewClientViewModel() 
        {
            item = new Clients();
        }
        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(FristName))
            {
                errors.Add("Imię jest wymagane.");
                isDataCorrect = false;
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                errors.Add("Nazwisko jest wymagane.");
                isDataCorrect = false;
            }
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                errors.Add("Numer telefonu jest wymagany.");
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
                estateEntities.Clients.Add(item);
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
