using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewClientViewModel : BaseDatabaseAdder<Clients>
    {
        #region Properties
        public string FristName
        {
            get
            {
                return item.FirstName;
            }
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => FristName);
            }
        }

        public string LastName
        {
            get
            {
                return item.LastName;
            }
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => LastName);
            }
        }

        public string PhoneNumber
        {
            get
            {
                return item.PhoneNumber;
            }
            set
            {
                item.PhoneNumber = value;
                OnPropertyChanged(() => PhoneNumber);
            }
        }

        public string Email
        {
            get
            {
                return item?.Email;
            }
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
            }
        }


        #endregion

        #region Constructor
        public AddNewClientViewModel() 
        {
            item = new Clients();
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            estateEntities.Clients.Add(item);
            estateEntities.SaveChanges();
        }
        #endregion
    }
}
