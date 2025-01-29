using realEstateDevelopment.Core;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;

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
        #endregion

        #region Commands
        public RealyCommand OpenAddNewClientCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
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
            throw new NotImplementedException();
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
