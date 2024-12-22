using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class SuppliersViewModel : LoadAllViewModel<Suppliers>
    {
        #region Commands
        public RealyCommand OpenAddNewSupplierCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewSupplierRequest;

        #endregion

        #region Constructor
        public SuppliersViewModel() 
        {
            OpenAddNewSupplierCommand = new RealyCommand(o =>
            {
                AddNewSupplierRequest?.Invoke();
            });
        }

        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
            var suppliers = await Task.Run(() => realEstateEntities.Suppliers.ToList());
            List = new ObservableCollection<Suppliers>(suppliers);
        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
