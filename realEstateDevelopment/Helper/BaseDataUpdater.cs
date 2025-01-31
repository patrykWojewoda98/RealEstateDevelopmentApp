using MVVMFirma.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.Windows.Input;

namespace realEstateDevelopment.Helper
{
    public abstract class BaseDataUpdater<T> : WorkspaceViewModel
    {
        #region Properties
        protected bool isDataCorrect;
        protected string potentialErrors;
        private T _selectedItem;
        public T SelectedItem {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        #endregion

        #region Db
        protected RealEstateEntities estateEntities;
        #endregion

        #region Item
        protected T item { get; set; }
        #endregion

        #region Command
        private BaseCommand _UpdateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                if (_UpdateCommand == null)
                    _UpdateCommand = new BaseCommand(() => UpdateAndClose());
                return _UpdateCommand;
            }

        }
        #endregion

        #region Constructor
        public BaseDataUpdater()
        {
            estateEntities = new RealEstateEntities();
        }
        #endregion

        #region Helpers
        public abstract void Update();

        public void UpdateAndClose()
        {
            Update();
            base.OnRequestClose();
        }
        public virtual void SaveHistoryOfChanges()
        {
            var change = new HistoryOfChanges();
            change.EmployeeId = MainViewModel.employee.EmployeeId;
            change.Operation = this.GetType().Name;
            change.DateAndTimeOfChange = DateTime.Now;
            estateEntities.HistoryOfChanges.Add(change);
            estateEntities.SaveChanges();
        }
        #endregion

    }
}
