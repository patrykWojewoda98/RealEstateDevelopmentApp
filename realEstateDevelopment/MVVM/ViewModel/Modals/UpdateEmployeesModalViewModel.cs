using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateEmployeesModalViewModel : BaseDataUpdater<Employees> 
    {
        #region Properties

        public int EmployeeID
        {
            get => item.EmployeeID;
            set
            {
                item.EmployeeID = value;
                OnPropertyChanged(() => EmployeeID);
            }
        }

        public string FirstName
        {
            get => item.FirstName;
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get => item.LastName;
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => LastName);
            }
        }

        public decimal Salary
        {
            get => (decimal)item.Salary;
            set
            {
                item.Salary = value;
                OnPropertyChanged(() => Salary);
            }
        }

        public string Position
        {
            get => item.Position;
            set
            {
                item.Position = value;
                OnPropertyChanged(() => Position);
            }
        }

        public string Department
        {
            get => item.Department;
            set
            {
                if (item.Department != null)
                {
                    item.Department=value;
                    OnPropertyChanged(() => Department);
                }
            }
        }

        public string Password
        {
            get => item.Password;
            set
            {
                if (item.Password != null)
                {
                    item.Password = value;
                    OnPropertyChanged(() => Password);
                }
            }
        }

        #endregion

        #region Constructor

        public UpdateEmployeesModalViewModel(Employees employee)
            : base()
        {
            item = employee;
            Password =  employee.Password;
            
        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                errors.Add("Imię pracownika jest wymagane.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                errors.Add("Nazwisko pracownika jest wymagane.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Position))
            {
                errors.Add("Stanowisko pracownika jest wymagane.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Department))
            {
                errors.Add("Dział pracownika jest wymagany.");
                isDataCorrect = false;
            }

            if (Salary <= 0)
            {
                errors.Add("Wynagrodzenie musi być większe od 0.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                errors.Add("Hasło pracownika jest wymagane.");
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
                var existingEmployee = estateEntities.Employees.FirstOrDefault(e => e.EmployeeID == EmployeeID);
                if (existingEmployee != null)
                {
                    existingEmployee.FirstName = item.FirstName;
                    existingEmployee.LastName = item.LastName;
                    existingEmployee.Position = item.Position;
                    existingEmployee.Department = item.Department;
                    existingEmployee.Salary = item.Salary;
                    existingEmployee.Password = item.Password;

                    estateEntities.SaveChanges();
                    SaveHistoryOfChanges();
                }
                else
                {
                    var errorModal = new ErrorModalView("Nie znaleziono pracownika do aktualizacji.");
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
            var updateEmployeeModal = new UpdateEmployeesModalView();
            updateEmployeeModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateEmployeeModal.Close();
            };

            updateEmployeeModal.ShowDialog();
        }

       

        #endregion
    }
}