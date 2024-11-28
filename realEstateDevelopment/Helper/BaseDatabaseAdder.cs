using MVVMFirma.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.ViewModel;
using System.Windows.Input;

namespace realEstateDevelopment.Helper
{
    public abstract class BaseDatabaseAdder<T> : WorkspaceViewModel
    {
        #region Db
        protected RealEstateEntities estateEntities;
        #endregion

        #region Item
        protected T item { get; set; }
        #endregion

        #region Command
        private BaseCommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                    _SaveCommand = new BaseCommand(() => SaveAndClose());
                return _SaveCommand;
            }

        }
        #endregion

        #region Constructor
        public BaseDatabaseAdder()
        {
            estateEntities = new RealEstateEntities();
        }
        #endregion

        #region Helpers
        public abstract void Save();

        public void SaveAndClose()
        {
            Save();
            base.OnRequestClose();
        }
        #endregion

    }
}
