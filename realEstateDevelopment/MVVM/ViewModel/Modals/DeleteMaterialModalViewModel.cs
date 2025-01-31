using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteMaterialModalViewModel : BaseDataDeleter<Materials>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int MaterialId
        {
            get => item.MaterialID;
            set
            {
                item.MaterialID = value;
                OnPropertyChanged(() => MaterialId);
            }
        }

        public string MaterialName
        {
            get => item.MaterialName;
            set
            {
                item.MaterialName = value;
                OnPropertyChanged(() => MaterialName);
            }
        }

        public string Type
        {
            get => item.Type;
            set
            {
                item.Type = value;
                OnPropertyChanged(() => Type);
            }
        }

        public decimal UnitPrice
        {
            get => item.UnitPrice;
            set
            {
                item.UnitPrice = value;
                OnPropertyChanged(() => UnitPrice);
            }
        }

        public string SupplierName
        {
            get => estateEntities.Suppliers.FirstOrDefault(s => s.SupplierID == item.SupplierID)?.CompanyName;
        }

        public string SupplierContact
        {
            get => estateEntities.Suppliers.FirstOrDefault(s => s.SupplierID == item.SupplierID)?.Contact;
        }

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        public RealyCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DeleteMaterialModalViewModel(Materials material)
            : base()
        {
            item = material;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Materials.FirstOrDefault(m => m.MaterialID == item.MaterialID);
            if (existingItem != null)
            {
                estateEntities.Materials.Remove(existingItem);
                estateEntities.SaveChanges();
                SaveHistoryOfChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono materiału do usunięcia.");
                errorModal.ShowDialog();
            }
        }

        private void ExecuteDelete(object parameter)
        {
            try
            {
                Delete();
                base.OnRequestClose();
            }
            catch (Exception ex)
            {
                var errorModal = new ErrorModalView($"Wystąpił błąd: {ex.Message}");
                errorModal.ShowDialog();
            }
        }

        private void CancelDelete(object parameter)
        {
            base.OnRequestClose();
        }
        #endregion
    }
}
