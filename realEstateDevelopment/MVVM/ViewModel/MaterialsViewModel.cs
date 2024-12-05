using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class MaterialsViewModel : LoadAllViewModel<MaterialsEntityForView>
    {


        #region Helpers
        public override async Task LoadAsync()
        {
            var material = realEstateEntities.Materials;
            var suppliers = realEstateEntities.Suppliers;

            List = new ObservableCollection<MaterialsEntityForView>(material.Join(suppliers,
                                                                                            m => m.SupplierID,
                                                                                            s => s.SupplierID,
                                                                                            (m, s) => new MaterialsEntityForView
                                                                                            {
                                                                                                MaterialId = m.MaterialID,
                                                                                                MaterialName = m.MaterialName,
                                                                                                Type = m.Type,
                                                                                                UnitPrice = m.UnitPrice,
                                                                                                SupplierName = s.CompanyName,
                                                                                                SupplierContact = s.Contact
                                                                                            })
                .ToList());



        }
        #endregion
    }
}
