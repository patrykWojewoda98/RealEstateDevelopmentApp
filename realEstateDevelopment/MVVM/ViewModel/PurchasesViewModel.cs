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
        #endregion
        #region Commands
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
            OpenAddNewPurchaseCommand = new RealyCommand(o =>
            {
                AddNewPurchaseRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
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
            throw new NotImplementedException();
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
