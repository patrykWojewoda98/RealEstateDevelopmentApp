using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteSupplierModalViewModel : BaseDataDeleter<Suppliers>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int SupplierID
        {
            get => item.SupplierID;
            set
            {
                item.SupplierID = value;
                OnPropertyChanged(() => SupplierID);
            }
        }

        public string CompanyName
        {
            get => item.CompanyName;
            set
            {
                item.CompanyName = value;
                OnPropertyChanged(() => CompanyName);
            }
        }

        public string Contact
        {
            get => item.Contact;
            set
            {
                item.Contact = value;
                OnPropertyChanged(() => Contact);
            }
        }

        public string Address
        {
            get => item.Address;
            set
            {
                item.Address = value;
                OnPropertyChanged(() => Address);
            }
        }

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        public RealyCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DeleteSupplierModalViewModel(Suppliers supplier)
            : base()
        {
            item = supplier;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.Suppliers.FirstOrDefault(s => s.SupplierID == item.SupplierID);
            if (existingItem != null)
            {
                // Usuń powiązane materiały
                var materialsToRemove = estateEntities.Materials
                    .Where(m => m.SupplierID == existingItem.SupplierID)
                    .ToList();
                foreach (var material in materialsToRemove)
                {
                    estateEntities.Materials.Remove(material);
                }

                estateEntities.Suppliers.Remove(existingItem);
                estateEntities.SaveChanges();
                SaveHistoryOfChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono dostawcy do usunięcia.");
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
