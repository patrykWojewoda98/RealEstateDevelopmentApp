﻿using realEstateDevelopment.Core;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ClientViewModel : LoadAllViewModel<ClientForView>
    {
        #region Properties
        private ClientForView _selectedItem;
        public ClientForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        private string _filterName;
        public string FilterName
        {
            get => _filterName;
            set
            {
                _filterName = value;
                OnPropertyChanged(() => FilterName);
                ApplyFiltersAsync();
            }
        }

        private string _filterSurname;
        public string FilterSurname
        {
            get => _filterSurname;
            set
            {
                _filterSurname = value;
                OnPropertyChanged(() => FilterSurname);
                ApplyFiltersAsync();
            }
        }

        private string _filterPesel;
        public string FilterPesel
        {
            get => _filterPesel;
            set
            {
                _filterPesel = value;
                OnPropertyChanged(() => FilterPesel);
                ApplyFiltersAsync();
            }
        }

        private string _filterEmail;
        public string FilterEmail
        {
            get => _filterEmail;
            set
            {
                _filterEmail = value;
                OnPropertyChanged(() => FilterEmail);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<ClientForView> _filteredList;
        public ObservableCollection<ClientForView> FilteredList
        {
            get => _filteredList;
            set
            {
                _filteredList = value;
                OnPropertyChanged(() => FilteredList);
            }
        }
        #endregion

        #region Commands
        public RealyCommand OpenAddNewClientCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        public RealyCommand ApplyFiltersCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewClientRequested;
        #endregion


        #region Constructor
        public ClientViewModel()
            : base()
        {
            OpenAddNewClientCommand = new RealyCommand(o =>
            {
                AddNewClientRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(o => ApplyFiltersAsync());
        }
        #endregion


        #region Helpers
        public override async Task LoadAsync()
        {
            var clients = from c in realEstateEntities.Clients
                          select new ClientForView
                          {
                            Name = c.FirstName,
                            Surname = c.LastName,
                            Id = c.ClientID,
                            Pesel = c.Pesel,
                            IdCardNumber = c.IdCardNumber,
                            IdCardSeries = c.IdCardSeries,
                            Email = c.Email,
                            PhoneNumber = c.PhoneNumber,
                          };

            List = new ObservableCollection<ClientForView>(clients);
        }

        public override Task ApplyFiltersAsync()
        {
            FilteredList = new ObservableCollection<ClientForView>(
                List.Where(c =>
                    (string.IsNullOrEmpty(FilterName) || c.Name.Contains(FilterName)) &&
                    (string.IsNullOrEmpty(FilterSurname) || c.Surname.Contains(FilterSurname)) &&
                    (string.IsNullOrEmpty(FilterPesel) || c.Pesel.Contains(FilterPesel)) &&
                     (string.IsNullOrEmpty(FilterEmail) || (!string.IsNullOrEmpty(c.Email) && c.Email.Contains(FilterEmail)))
                ));
            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item);
            }
            Console.WriteLine("Prubuje");
            return Task.CompletedTask;
        }

        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is ClientForView selectedClient)
            {
                var modal = new DeleteClientModalView();

                
                DeleteClientModalViewModel deleteClientModalViewModel = new DeleteClientModalViewModel(
                                        realEstateEntities.Clients.First(c => c.ClientID == selectedClient.Id)
                    );
                deleteClientModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteClientModalViewModel;

                modal.Show();

            }
        }

        private bool CanExecuteDeleteSelected(object parameter)
        {
            return SelectedItem != null;
        }

        #endregion
    }
}
