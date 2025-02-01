using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private decimal? _filterPriceFrom;
        public decimal? FilterPriceFrom
        {
            get => _filterPriceFrom;
            set
            {
                _filterPriceFrom = value;
                OnPropertyChanged(() => FilterPriceFrom);
                ApplyFiltersAsync();
            }
        }

        private decimal? _filterPriceTo;
        public decimal? FilterPriceTo
        {
            get => _filterPriceTo;
            set
            {
                _filterPriceTo = value;
                OnPropertyChanged(() => FilterPriceTo);
                ApplyFiltersAsync();
            }
        }

        private string _filterMaterialType;
        public string FilterMaterialType
        {
            get => _filterMaterialType;
            set
            {
                _filterMaterialType = value;
                OnPropertyChanged(() => FilterMaterialType);
                ApplyFiltersAsync();
            }
        }

        private string _filterSupplierName;
        public string FilterSupplierName
        {
            get => _filterSupplierName;
            set
            {
                _filterSupplierName = value;
                OnPropertyChanged(() => FilterSupplierName);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<MaterialsEntityForView> _filteredList;
        public ObservableCollection<MaterialsEntityForView> FilteredList
        {
            get => _filteredList;
            set
            {
                _filteredList = value;
                OnPropertyChanged(() => FilteredList);
            }
        }
        #endregion

        #region Commands
        public RealyCommand ApplyFiltersCommand { get; set; }
        public RealyCommand OpenAddNewMaterialCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewMaterialRequested;
        #endregion

        public MaterialsViewModel()
            :base()
        {
            _filterPriceTo = realEstateEntities.Materials.Max(m => m.UnitPrice);
            OpenAddNewMaterialCommand = new RealyCommand(o =>
            {
                AddNewMaterialRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
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
            FilteredList = new ObservableCollection<MaterialsEntityForView>(
                List.Where(m =>
                    (!FilterPriceFrom.HasValue || m.UnitPrice >= FilterPriceFrom) &&
                    (!FilterPriceTo.HasValue || m.UnitPrice <= FilterPriceTo) &&
                    (string.IsNullOrEmpty(FilterMaterialType) || m.Type.Contains(FilterMaterialType)) &&
                    (string.IsNullOrEmpty(FilterSupplierName) || m.SupplierName.Contains(FilterSupplierName))
                ));
            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item);
            }
            return Task.CompletedTask;
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
