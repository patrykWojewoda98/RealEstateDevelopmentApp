using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class PurchasesViewModel : LoadAllViewModel<Purchases>
    {

        #region Commands
        public RealyCommand OpenAddNewPurchaseCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewPurchaseRequested;
        #endregion

        #region Constructor
        public PurchasesViewModel()
            :base()
        {
            OpenAddNewPurchaseCommand = new RealyCommand(o =>
            {
                AddNewPurchaseRequested?.Invoke();
            });
        }
        #endregion
        public override async Task LoadAsync()
        {
            var purchases = await Task.Run(() => realEstateEntities.Purchases.ToList());
            List = new ObservableCollection<Purchases>(purchases);
        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
