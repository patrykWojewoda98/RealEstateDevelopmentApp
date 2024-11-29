using MVVMFirma.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public abstract class LoadAllViewModel<T> : WorkspaceViewModel 
    {
        #region Fields
        protected readonly RealEstateDeveloperDBEntities realEstateEntities;
        private BaseCommand _LoadCommand;
        private ObservableCollection<T> _List;
        #endregion

        #region Properties
        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                    _LoadCommand = new BaseCommand(() => LoadAsync());
                return _LoadCommand;
            }
        }

        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                    LoadAsync();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);
            }
        }
            #endregion

            #region Constructor
            public LoadAllViewModel()
            {
                realEstateEntities = new RealEstateDeveloperDBEntities();
            }
        #endregion

        #region Helpers
        public abstract Task LoadAsync(); 
        #endregion
    }

}

