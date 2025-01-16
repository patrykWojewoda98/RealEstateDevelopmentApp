using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class SuppliersViewModel : LoadAllViewModel<SuppliersEntityForView>
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
            var query = from s in realEstateEntities.Suppliers
                        select new SuppliersEntityForView
                        {
                            SupplierID = s.SupplierID,
                            Address = s.Address,
                            Contact = s.Contact,
                            CompanyName = s.CompanyName,
                        };
        

            var result = await query.ToListAsync();
            List = new ObservableCollection<SuppliersEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
