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

        public string Pesel
        {
            get
            {
                return item.Pesel;
            }
            set
            {
                item.Pesel = value;
                OnPropertyChanged(() => Pesel);
            }
        }

        public int IdCardNumber
        {
            get
            {
                return item.IdCardNumber;
            }
            set
            {
                item.IdCardNumber = value;
                OnPropertyChanged(() => IdCardNumber);
            }
        }

        public string IdCardSeries
        {
            get
            {
                return item.IdCardSeries;
            }
            set
            {
                item.IdCardSeries = value;
                OnPropertyChanged(() => IdCardSeries);
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
            if (string.IsNullOrWhiteSpace(Pesel))
            {
                errors.Add("Pesel jest wymagany.");
                isDataCorrect = false;
            }
            if (99999>IdCardNumber)
            {
                errors.Add("Numer dowodu osobistego nie może być liczbą 5cyfrową lub mniej.");
                isDataCorrect = false;
            }
            if (999999 < IdCardNumber)
            {
                errors.Add("Numer dowodu osobistego nie może być liczbą 7cyfrową lub wiecej.");
                isDataCorrect = false;
            }
            if (string.IsNullOrWhiteSpace(IdCardSeries) || IdCardSeries.Length != 3)
            {
                errors.Add("Seria dowodu osobistego musi zawierać dokładnie 3 znaki\"");
                isDataCorrect = false;
            }

            potentialErrors = string.Join(Environment.NewLine, errors);
        }
        public override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FristName):
                    return string.IsNullOrWhiteSpace(FristName) ? "Imię jest wymagane." : string.Empty;

                case nameof(LastName):
                    return string.IsNullOrWhiteSpace(LastName) ? "Nazwisko jest wymagane." : string.Empty;

                case nameof(PhoneNumber):
                    return string.IsNullOrWhiteSpace(PhoneNumber) ? "Numer telefonu jest wymagany." : string.Empty;

                case nameof(Pesel):
                    return string.IsNullOrWhiteSpace(Pesel) ? "Pesel jest wymagany." : string.Empty;

                case nameof(IdCardNumber):
                    return IdCardNumber <= 99999 ? "Numer dowodu osobistego nie może być liczbą 5-cyfrową lub mniej." :
                           IdCardNumber > 999999 ? "Numer dowodu osobistego nie może być liczbą 7-cyfrową lub więcej." :
                           string.Empty;

                case nameof(IdCardSeries):
                    return string.IsNullOrWhiteSpace(IdCardSeries) || IdCardSeries.Length != 3
                        ? "Seria dowodu osobistego musi zawierać dokładnie 3 znaki."
                        : string.Empty;

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
