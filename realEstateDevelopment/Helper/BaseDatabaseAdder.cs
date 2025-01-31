using MVVMFirma.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace realEstateDevelopment.Helper
{
    public abstract class BaseDatabaseAdder<T> : WorkspaceViewModel, IDataErrorInfo
    {
        #region Properties
        protected bool isDataCorrect;
        protected string potentialErrors;
        #endregion

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

        public virtual string ValidateProperty(string propertyName)
        {
            return string.Empty;
        }

        public void SaveAndClose()
        {
            Save();
            base.OnRequestClose();
        }

        public string Error => string.Empty;

        public string this[string columnName] => ValidateProperty(columnName);

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
