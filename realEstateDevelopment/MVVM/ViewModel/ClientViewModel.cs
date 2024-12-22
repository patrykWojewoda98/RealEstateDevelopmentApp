using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ClientViewModel : LoadAllViewModel<Clients>
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
            var clients = await Task.Run(() => realEstateEntities.Clients.ToList());
            List = new ObservableCollection<Clients>(clients);
        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
