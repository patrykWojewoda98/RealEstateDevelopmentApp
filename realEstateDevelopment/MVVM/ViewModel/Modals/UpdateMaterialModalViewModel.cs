using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System.Collections.Generic;
using System;
using System.Linq;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class UpdateMaterialModalViewModel : BaseDataUpdater<Materials>
    {
        #region Properties


        private SuppliersEntityForView _selectedSupplier;


        public ObservableCollection<SuppliersEntityForView> AvailableSupplier { get; set; }

        public SuppliersEntityForView SelectedSupplier
        {
            get
            {
                return _selectedSupplier;
            }

            set
            {
                _selectedSupplier = value;
                if (_selectedSupplier != null)
                {
                    item.SupplierID = _selectedSupplier.SupplierID;
                }
                OnPropertyChanged(() => SelectedSupplier);
            }
        }

        public int MaterialID
        {
            get => item.MaterialID;
            set
            {
                item.MaterialID = value;
                OnPropertyChanged(() => MaterialID);
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

        public int SupplierId
        {
            get => item.SupplierID;
            set
            {
                item.SupplierID = value;
                OnPropertyChanged(() => SupplierId);
            }
        }

        public string SupplierName
        {
            get => item.Suppliers?.CompanyName ?? string.Empty;
            set
            {
                if (item.Suppliers != null)
                {
                    item.Suppliers.CompanyName = value;
                    OnPropertyChanged(() => SupplierName);
                }
            }
        }

        public string SupplierContact
        {
            get => item.Suppliers?.Contact ?? string.Empty;
            set
            {
                if (item.Suppliers != null)
                {
                    item.Suppliers.Contact = value;
                    OnPropertyChanged(() => SupplierContact);
                }
            }
        }

        #endregion

        #region Constructor

        public UpdateMaterialModalViewModel(Materials material)
            : base()
        {
            item = material;
            AvailableSupplier = new ObservableCollection<SuppliersEntityForView>();
            LoadSuppliers();
            SelectedSupplier = AvailableSupplier.FirstOrDefault(s=>s.SupplierID == SupplierId);
        }

        #endregion

        #region Validation

        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();

            if (MaterialID <= 0)
            {
                errors.Add("ID materiału nie może być mniejsze lub równe 0.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(MaterialName))
            {
                errors.Add("Nazwa materiału jest wymagana.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(Type))
            {
                errors.Add("Typ materiału jest wymagany.");
                isDataCorrect = false;
            }

            if (UnitPrice <= 0)
            {
                errors.Add("Cena jednostkowa musi być większa od 0.");
                isDataCorrect = false;
            }

            if (SupplierId <= 0)
            {
                errors.Add("ID dostawcy nie może być mniejsze lub równe 0.");
                isDataCorrect = false;
            }

            potentialErrors = string.Join(Environment.NewLine, errors);
        }

        #endregion

        #region Helpers

        public override void Update()
        {
            Validate();
            if (isDataCorrect)
            {
                var existingItem = estateEntities.Materials.FirstOrDefault(m => m.MaterialID == MaterialID);
                if (existingItem != null)
                {
                    existingItem.MaterialName = item.MaterialName;
                    existingItem.Type = item.Type;
                    existingItem.UnitPrice = item.UnitPrice;
                    existingItem.SupplierID = item.SupplierID;

                    estateEntities.SaveChanges();
                }
                else
                {
                    var errorModal = new ErrorModalView("Nie znaleziono materiału do aktualizacji.");
                    errorModal.ShowDialog();
                }
            }
            else
            {
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }

        public void ShowMessageBox(string message)
        {
            var updateMaterialModal = new UpdateMaterialModalView();
            updateMaterialModal.DataContext = this;

            this.RequestClose += (sender, args) =>
            {
                updateMaterialModal.Close();
            };

            updateMaterialModal.ShowDialog();
        }

        private void LoadSuppliers()
        {
            var suppliers = (from s in estateEntities.Suppliers
                              select new SuppliersEntityForView
                              {
                                  SupplierID = s.SupplierID,
                                  Address = s.Address,
                                  CompanyName = s.CompanyName,
                                  Contact = s.Contact,
                              }).ToList();

            foreach (var supplier in suppliers)
            {
                AvailableSupplier.Add(supplier);
            }
        }

        #endregion
    }
}
