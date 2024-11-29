using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewSupplierViewModel : BaseDatabaseAdder<Suppliers>
    {
        #region Properties
        public string CompanyName
        {
            get
            {
                return item.CompanyName;
            }
            set
            {
                item.CompanyName = value;
                OnPropertyChanged(() => CompanyName);
            }
        }
        public string Contact
        {
            get
            {
                return item.Contact;
            }
            set
            {
                item.Contact = value;
                OnPropertyChanged(() => Contact);
            }
        }

        public string Address
        {
            get
            {
                return item.Address;
            }
            set
            {
                item.Address = value;
                OnPropertyChanged(() => Address);
            }
        }
        public string MaterialType
        {
            get
            {
                return item.MaterialType;
            }
            set
            {
                item.MaterialType = value;
                OnPropertyChanged(() => MaterialType);
            }
        }

        #endregion

        #region Constructor
        public AddNewSupplierViewModel()
        {
            item = new Suppliers();
        }
        #endregion
        public override void Save()
        {
            estateEntities.Suppliers.Add(item);
            estateEntities.SaveChanges();
        }
    }
}
