﻿using MVVMFirma.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.ViewModel;
using System.Windows.Input;

namespace realEstateDevelopment.Helper
{
    public abstract class BaseDataDeleter<T> : WorkspaceViewModel
    {
        #region Properties
        protected bool isDataCorrect;
        protected string potentialErrors;
        private T _selectedItem;
        public T SelectedItem
        {
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
        private BaseCommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                    _DeleteCommand = new BaseCommand(() => ExecuteDelete());
                return _DeleteCommand;
            }

        }
        #endregion

        #region Constructor
        public BaseDataDeleter()
        {
            estateEntities = new RealEstateEntities();
        }
        #endregion

        #region Helpers
        public abstract void Delete();

        public void ExecuteDelete()
        {
            Delete();
            base.OnRequestClose();
        }
        #endregion

    }
}
