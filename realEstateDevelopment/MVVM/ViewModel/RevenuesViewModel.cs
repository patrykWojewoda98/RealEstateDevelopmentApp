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

        private decimal? _filterAmountFrom;
        public decimal? FilterAmountFrom
        {
            get => _filterAmountFrom;
            set
            {
                _filterAmountFrom = value;
                OnPropertyChanged(() => FilterAmountFrom);
                ApplyFiltersAsync();
            }
        }

        private decimal? _filterAmountTo;
        public decimal? FilterAmountTo
        {
            get => _filterAmountTo;
            set
            {
                _filterAmountTo = value;
                OnPropertyChanged(() => FilterAmountTo);
                ApplyFiltersAsync();
            }
        }

        private DateTime? _filterDate;
        public DateTime? FilterDate
        {
            get => _filterDate;
            set
            {
                _filterDate = value;
                OnPropertyChanged(() => FilterDate);
                ApplyFiltersAsync();
            }
        }

        private string _filterProjectName;
        public string FilterProjectName
        {
            get => _filterProjectName;
            set
            {
                _filterProjectName = value;
                OnPropertyChanged(() => FilterProjectName);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<RevenuesEntityForView> _filteredList;
        public ObservableCollection<RevenuesEntityForView> FilteredList
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
        public RealyCommand OpenAddNewRevenueCommand { get; set; }
        public RealyCommand DeleteSelectedCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewRevenueRequested;
        #endregion

        public RevenuesViewModel()
            : base()
        {
            _filterAmountTo = realEstateEntities.Revenues.Max(r => r.Amount);
            OpenAddNewRevenueCommand = new RealyCommand(o =>
            {
                AddNewRevenueRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
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
            var filtered = List.Where(item =>
                (!FilterAmountFrom.HasValue || item.RevenueAmount >= FilterAmountFrom.Value) &&
                (!FilterAmountTo.HasValue || item.RevenueAmount <= FilterAmountTo.Value) &&
                (!FilterDate.HasValue || item.RevenueDate <= FilterDate) &&
                (string.IsNullOrEmpty(FilterProjectName) || item.ProjectName.Contains(FilterProjectName))
            );

            FilteredList = new ObservableCollection<RevenuesEntityForView>(filtered);

            List.Clear();
            foreach (var item in FilteredList)
            {
                List.Add(item);
            }

            return Task.CompletedTask;
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
