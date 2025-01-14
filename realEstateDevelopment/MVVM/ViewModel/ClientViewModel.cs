using realEstateDevelopment.Core;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;
using realEstateDevelopment.MVVM.Model.EntitiesForView;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ClientViewModel : LoadAllViewModel<ClientForView>
    {
        #region Commands
        public RealyCommand OpenAddNewClientCommand { get; set; }
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


        #endregion
    }
}
