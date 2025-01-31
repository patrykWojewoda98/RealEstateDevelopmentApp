using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class MaterialsViewModel : LoadAllViewModel<MaterialsEntityForView>
    {
        #region Properties
        private MaterialsEntityForView _selectedItem;
        public MaterialsEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        #endregion

        #region Commands
        public RealyCommand OpenAddNewMaterialCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewMaterialRequested;
        #endregion

        public MaterialsViewModel()
            :base()
        {
            OpenAddNewMaterialCommand = new RealyCommand(o =>
            {
                AddNewMaterialRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
        }

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from m in realEstateEntities.Materials
                        join s in realEstateEntities.Suppliers on m.SupplierID equals s.SupplierID
                        select new MaterialsEntityForView
                        {
                            MaterialId = m.MaterialID,
                            MaterialName = m.MaterialName,
                            Type = m.Type,
                            UnitPrice = m.UnitPrice,
                            SupplierName = s.CompanyName,
                            SupplierContact = s.Contact
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<MaterialsEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is MaterialsEntityForView selected)
            {
                var modal = new DeleteMaterialModalView();
                DeleteMaterialModalViewModel deleteMaterialModalViewModel = new DeleteMaterialModalViewModel(
                                            realEstateEntities.Materials.First(m => m.MaterialID == SelectedItem.MaterialId));
                deleteMaterialModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteMaterialModalViewModel;
                modal.Show();

            }
        }


        private bool CanExecuteDeleteSelected(object parameter)
        {
            return SelectedItem != null; // Polecenie dostępne tylko, jeśli coś jest zaznaczone.
        }
        #endregion
    }
}
