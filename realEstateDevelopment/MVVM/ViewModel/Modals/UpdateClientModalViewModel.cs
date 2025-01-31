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
    public class UpdateClientModalViewModel : BaseDataUpdater<Clients>
    {
    #region Propeties

        public int Id
        {
            get
            {
                return item.ClientID;
            }
            set
            {
                item.ClientID = value;
                OnPropertyChanged(() => Id);
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

    public string Surname
    {
        get
        {
            return item.LastName;
        }
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => Surname);
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

        public string Email
        {
            get
            {
                return item.Email;
            }
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
            }
        }


    
    #endregion

    #region Constructor
    public UpdateClientModalViewModel(Clients client)
        : base()
    {
        item = client;
        Console.WriteLine(client.FirstName);
    }
    #endregion

    #region Validation
    public void Validate()
    {
        isDataCorrect = true;
        var errors = new List<string>();
        if (Id <= 0)
        {
            errors.Add("Id klienta nie może być mniejszy lub równy 0");
            isDataCorrect = false;
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            errors.Add("Imie klienta jest wymagane.");
            isDataCorrect = false;
        }

        if (string.IsNullOrWhiteSpace(Surname))
            {
            errors.Add("Nazwisko klienta jest wymagane");
            isDataCorrect = false;
        }

        if (string.IsNullOrWhiteSpace(Pesel))
            {
            errors.Add("Pesel klienta jest wymagany");
            isDataCorrect = false;
        }

        if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
            errors.Add("Numer telefonu klienta jest wymagany");
            isDataCorrect = false;
        }
        if (IdCardNumber <=0)
        {
            errors.Add("Numer dowodu klienta nie moze być mniejszy od Zera");
            isDataCorrect = false;
        }
            if (string.IsNullOrWhiteSpace(IdCardSeries))
            {
                errors.Add("Seria dowodu klienta jest wymagana");
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
            var existingItem = estateEntities.Clients.FirstOrDefault(c => c.ClientID == item.ClientID);
            if (existingItem != null)
            {
                existingItem.ClientID = item.ClientID;
                existingItem.FirstName = item.FirstName;
                existingItem.LastName = item.LastName;
                existingItem.Pesel = item.Pesel;
                existingItem.PhoneNumber = item.PhoneNumber;
                existingItem.Email = item.Email;
                existingItem.IdCardNumber = item.IdCardNumber;
                existingItem.IdCardSeries = item.IdCardSeries;

                estateEntities.SaveChanges();
                    SaveHistoryOfChanges();
                }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono elementu do aktualizacji.");
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
        // Zamiast wyświetlać alert, otwórz modal.
        var updateClientModal = new UpdateClientModalView(); // To powinien być Twój UserControl/Window
            updateClientModal.DataContext = this;

        this.RequestClose += (sender, args) =>
        {
            updateClientModal.Close();
        };

            updateClientModal.ShowDialog(); // Jeśli to jest Window
    }

    #endregion
}
    }
