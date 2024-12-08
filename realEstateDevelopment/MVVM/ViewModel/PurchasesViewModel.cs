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
        public override async Task LoadAsync()
        {
            var purchases = await Task.Run(() => realEstateEntities.Purchases.ToList());
            List = new ObservableCollection<Purchases>(purchases);
        }
    }
}
