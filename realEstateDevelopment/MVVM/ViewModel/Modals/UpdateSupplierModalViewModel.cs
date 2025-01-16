using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateSupplierModalViewModel : BaseDataUpdater<Suppliers>
    {
        #region Propeties

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
        #endregion

        #region Constructor
        public UpdateSupplierModalViewModel(Suppliers supplier)
            : base()
        {
            item = supplier;
        }
        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            // Walidacja CompanyName
            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                errors.Add("Nazwa firmy jest wymagana.");
                isDataCorrect = false;
            }

            // Walidacja Contact
            if (string.IsNullOrWhiteSpace(Contact))
            {
                errors.Add("Kontakt do dostawcy jest wymagany.");
                isDataCorrect = false;
            }
            else if (Contact.Length < 6)
            {
                errors.Add("Kontakt do dostawcy powinien mieć przynajmniej 6 znaków.");
                isDataCorrect = false;
            }

            // Walidacja Address
            if (string.IsNullOrWhiteSpace(Address))
            {
                errors.Add("Adres dostawcy jest wymagany.");
                isDataCorrect = false;
            }

            // Zbierz potencjalne błędy do jednej zmiennej
            potentialErrors = string.Join(Environment.NewLine, errors);
        }

        #endregion


        #region Helpers
        public override void Update()
        {
            // Najpierw walidacja
            Validate();
            if (isDataCorrect)
            {
                // Znajdź istniejący element w bazie danych
                var existingItem = estateEntities.Suppliers.FirstOrDefault(s => s.SupplierID == item.SupplierID);

                if (existingItem != null)
                {
                    // Zaktualizuj właściwości
                    existingItem.CompanyName = item.CompanyName;
                    existingItem.Contact = item.Contact;
                    existingItem.Address = item.Address;

                    // Zapisz zmiany w bazie danych
                    estateEntities.SaveChanges();
                }
                else
                {
                    // Wyświetl błąd, jeśli element nie został znaleziony
                    var errorModal = new ErrorModalView("Nie znaleziono dostawcy do aktualizacji.");
                    errorModal.ShowDialog();
                }
            }
            else
            {
                // Wyświetl błędy walidacji w oknie błędu
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }


        public void ShowMessageBox(string message)
        {
            // Zamiast wyświetlać alert, otwórz modal.
            var updateSupplierModal = new UpdateSupplierModalView(); // To powinien być Twój UserControl/Window
            updateSupplierModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateSupplierModal.Close();
            };

            updateSupplierModal.ShowDialog(); // Jeśli to jest Window
        }

        #endregion
    }
}

