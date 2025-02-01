using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.View.Modals;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class PurchasesViewModel : LoadAllViewModel<PurchasesEntityForView>
    {
        #region Properties
        private PurchasesEntityForView _selectedItem;
        public PurchasesEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        private string _typeOfPurchaseFilter;
        public string TypeOfPurchaseFilter
        {
            get => _typeOfPurchaseFilter;
            set
            {
                _typeOfPurchaseFilter = value;
                OnPropertyChanged(() => TypeOfPurchaseFilter);
                ApplyFiltersAsync();
            }
        }

        private decimal? _minAmount;
        public decimal? MinAmount
        {
            get => _minAmount;
            set
            {
                _minAmount = value;
                OnPropertyChanged(() => MinAmount);
                ApplyFiltersAsync();
            }
        }

        private decimal? _maxAmount;
        public decimal? MaxAmount
        {
            get => _maxAmount;
            set
            {
                _maxAmount = value;
                OnPropertyChanged(() => MaxAmount);
                ApplyFiltersAsync();
            }
        }

        private DateTime? _purchaseDateFromFilter;
        public DateTime? PurchaseDateFromFilter
        {
            get => _purchaseDateFromFilter;
            set
            {
                _purchaseDateFromFilter = value;
                OnPropertyChanged(() => PurchaseDateFromFilter);
                ApplyFiltersAsync();
            }
        }

        private DateTime? _purchaseDateToFilter;
        public DateTime? PurchaseDateToFilter
        {
            get => _purchaseDateToFilter;
            set
            {
                _purchaseDateToFilter = value;
                OnPropertyChanged(() => PurchaseDateToFilter);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<PurchasesEntityForView> _filteredList;
        public ObservableCollection<PurchasesEntityForView> FilteredList
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
        public RealyCommand OpenAddNewPurchaseCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewPurchaseRequested;
        #endregion

        #region Constructor
        public PurchasesViewModel()
            :base()
        {
            _maxAmount = realEstateEntities.Purchases.Max(p => p.Amount);
            OpenAddNewPurchaseCommand = new RealyCommand(o =>
            {
                AddNewPurchaseRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
        }
        #endregion
        public override async Task LoadAsync()
        {
            var purchases = from p in realEstateEntities.Purchases
                            select new PurchasesEntityForView
                            {
                                PurchaseID = p.PurchaseID,
                                TypeOfPurchase = p.TypeOfPurchase,
                                Amount = p.Amount,
                                PurchaseDate = p.PurchaseDate
                            };
            List = new ObservableCollection<PurchasesEntityForView>(purchases);
        }

        public override Task ApplyFiltersAsync()
        {
            FilteredList = new ObservableCollection<PurchasesEntityForView>(
                List.Where(p =>
                    (string.IsNullOrEmpty(TypeOfPurchaseFilter) || p.TypeOfPurchase.Contains(TypeOfPurchaseFilter)) &&
                    (!MinAmount.HasValue || p.Amount >= MinAmount) &&
                    (!MaxAmount.HasValue || p.Amount <= MaxAmount) 
                ));
            List.Clear();
            foreach(var item  in FilteredList)
            {
                List.Add(item);
            }
            return Task.CompletedTask;
        }

        
        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is PurchasesEntityForView selected)
            {
                var modal = new DeletePurchaseModalView();
                DeletePurchaseModalViewModel deletePurchaseModalViewModel = new DeletePurchaseModalViewModel(
                                            realEstateEntities.Purchases.First(p => p.PurchaseID == SelectedItem.PurchaseID));
                deletePurchaseModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deletePurchaseModalViewModel;
                modal.Show();

            }
        }


        private bool CanExecuteDeleteSelected(object parameter)
        {
            return SelectedItem != null; // Polecenie dostępne tylko, jeśli coś jest zaznaczone.
        }
    }
}
