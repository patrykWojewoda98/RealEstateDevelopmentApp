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
    public class RevenuesViewModel : LoadAllViewModel<RevenuesEntityForView>
    {

        #region Properties
        private RevenuesEntityForView _selectedItem;
        public RevenuesEntityForView SelectedItem
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
        public RealyCommand OpenAddNewRevenueCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewRevenueRequested;
        #endregion

        public RevenuesViewModel()
            : base()
        {
            OpenAddNewRevenueCommand = new RealyCommand(o =>
            {
                AddNewRevenueRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
        }

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from r in realEstateEntities.Revenues
                        join p in realEstateEntities.Projects on r.ProjectID equals p.ProjectID
                        select new RevenuesEntityForView
                        {
                            Id = r.RevenueID,
                            ProjectName = p.ProjectName,
                            Address = p.Location,
                            RevenueType = r.RevenueType,
                            RevenueAmount = r.Amount,
                            RevenueDate = r.RevenueDate
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<RevenuesEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        private void ExecuteDeleteSelected(object parameter)
        {
            if (SelectedItem is RevenuesEntityForView selected)
            {
                var modal = new DeleteRevenueModalView();
                DeleteRevenueModalViewModel deleteRevenueModalViewModel = new DeleteRevenueModalViewModel(
                                            realEstateEntities.Revenues.First(r => r.RevenueID == SelectedItem.Id));
                deleteRevenueModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteRevenueModalViewModel;
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
