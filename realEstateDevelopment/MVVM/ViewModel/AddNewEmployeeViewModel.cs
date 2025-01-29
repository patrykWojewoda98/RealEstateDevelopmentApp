using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewEmployeeViewModel : BaseDatabaseAdder<Employees>
    {
        #region Properties
        private string _originalPassword;
        private string _repeatedPassword;

        public string OriginalPassword
        {
            get => _originalPassword;
            set
            {
                _originalPassword = value;
                OnPropertyChanged(nameof(OriginalPassword));
            }
        }

        public string RepeatedPassword
        {
            get => _repeatedPassword;
            set
            {
                _repeatedPassword = value;
                OnPropertyChanged(nameof(RepeatedPassword));
            }
        }

        public string Name
        {
            get
            {
                return item.FirstName;
            }
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => Name);
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

        public string Position
        {
            get
            {
                return item.Position;
            }
            set
            {
                item.Position = value;
                OnPropertyChanged(() => Position);
            }
        }

        public string Department
        {
            get
            {
                return item.Department;
            }
            set
            {
                item.Department = value;
                OnPropertyChanged(() => Department);
            }
        }

        public decimal Salary
        {
            get
            {
                return item.Salary??0;
            }
            set
            {
                item.Salary = value;
                OnPropertyChanged(() => Salary);
            }
        }
        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Constructor
        public AddNewEmployeeViewModel()
            :base()
        {
            item = new Employees();
            OriginalPassword = string.Empty;
            RepeatedPassword = string.Empty;
        }
        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();
            var db = new RealEstateEntities();


            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add("Imię jest wymagane.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                errors.Add("Nazwisko jest wymagane.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Position))
            {
                errors.Add("Pozycja pracownika jest wymagana.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Department))
            {
                errors.Add("Departament jest wymagany.");
                isDataCorrect = false;
            }

            if (Salary<=0)
            {
                errors.Add("Wypłata nie może wynośić 0 lub mniej.");
                isDataCorrect = false;
            }

            if (OriginalPassword.Length<5)
            {
                errors.Add("Hasło Nie może mieć mniej niż 5 znaków");
                isDataCorrect = false;
            }

            if (!OriginalPassword.Equals(RepeatedPassword))
            {
                errors.Add("Podane hasła nie są jednakowe");
                isDataCorrect = false;
            }
            else
            {
                item.Password =OriginalPassword;
            }
            potentialErrors = string.Join(Environment.NewLine, errors);
        }
        public override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    return string.IsNullOrWhiteSpace(Name) ? "Imię jest wymagane." : string.Empty;

                case nameof(LastName):
                    return string.IsNullOrWhiteSpace(LastName) ? "Nazwisko jest wymagane." : string.Empty;

                case nameof(Position):
                    return string.IsNullOrWhiteSpace(Position) ? "Pozycja pracownika jest wymagana." : string.Empty;

                case nameof(Department):
                    return string.IsNullOrWhiteSpace(Department) ? "Departament jest wymagany." : string.Empty;

                case nameof(Salary):
                    return Salary <= 0 ? "Wypłata nie może wynosić 0 lub mniej." : string.Empty;

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
                estateEntities.Employees.Add(item);
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
