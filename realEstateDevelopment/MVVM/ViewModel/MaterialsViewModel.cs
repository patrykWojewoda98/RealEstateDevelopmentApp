using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class MaterialsViewModel : LoadAllViewModel<MaterialsEntityForView>
    {
        #region Commands
        public RealyCommand OpenAddNewMaterialCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewMaterialRequested;
        #endregion

        public MaterialsViewModel()
            :base()
        {
            OpenAddNewMaterialCommand = new RealyCommand(o =>
            {
                AddNewMaterialRequested?.Invoke();
            });
        }

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from m in realEstateEntities.Materials
                        join s in realEstateEntities.Suppliers on m.SupplierID equals s.SupplierID
                        select new MaterialsEntityForView
                        {
                            MaterialId = m.MaterialID,
                            MaterialName = m.MaterialName,
                            Type = m.Type,
                            UnitPrice = m.UnitPrice,
                            SupplierName = s.CompanyName,
                            SupplierContact = s.Contact
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<MaterialsEntityForView>(result);
        }
        #endregion
    }
}
