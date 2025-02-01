using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
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
    public class SuppliersViewModel : LoadAllViewModel<SuppliersEntityForView>
    {
        #region Properties
        private SuppliersEntityForView _selectedItem;
        public SuppliersEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        private string _companyNameFilter;
        public string CompanyNameFilter
        {
            get => _companyNameFilter;
            set
            {
                _companyNameFilter = value;
                OnPropertyChanged(() => CompanyNameFilter);
                ApplyFiltersAsync();
            }
        }

        private string _contactFilter;
        public string ContactFilter
        {
            get => _contactFilter;
            set
            {
                _contactFilter = value;
                OnPropertyChanged(() => ContactFilter);
                ApplyFiltersAsync();
            }
        }

        private string _addressFilter;
        public string AddressFilter
        {
            get => _addressFilter;
            set
            {
                _addressFilter = value;
                OnPropertyChanged(() => AddressFilter);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<SuppliersEntityForView> _filteredList;
        public ObservableCollection<SuppliersEntityForView> FilteredList
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
        public RealyCommand OpenAddNewSupplierCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        public RealyCommand ApplyFiltersCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewSupplierRequest;

        #endregion

        #region Constructor
        public SuppliersViewModel() 
        {
            OpenAddNewSupplierCommand = new RealyCommand(o =>
            {
                AddNewSupplierRequest?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
        }

        #endregion

        #region Helpers

        public override async Task LoadAsync()
        {
            var query = from s in realEstateEntities.Suppliers
                        select new SuppliersEntityForView
                        {
                            SupplierID = s.SupplierID,
                            Address = s.Address,
                            Contact = s.Contact,
                            CompanyName = s.CompanyName,
                        };
        

            var result = await query.ToListAsync();
            List = new ObservableCollection<SuppliersEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            FilteredList = new ObservableCollection<SuppliersEntityForView>(
                List.Where(s =>
                    (string.IsNullOrEmpty(CompanyNameFilter) || s.CompanyName.Contains(CompanyNameFilter)) &&
                    (string.IsNullOrEmpty(ContactFilter) || s.Contact.Contains(ContactFilter)) &&
                    (string.IsNullOrEmpty(AddressFilter) || s.Address.Contains(AddressFilter))
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
            if (SelectedItem is SuppliersEntityForView selected)
            {
                var modal = new DeleteSupplierModalView();
                DeleteSupplierModalViewModel deleteSupplierModalViewModel = new DeleteSupplierModalViewModel(
                                            realEstateEntities.Suppliers.First(s => s.SupplierID == SelectedItem.SupplierID));
                deleteSupplierModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteSupplierModalViewModel;
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
