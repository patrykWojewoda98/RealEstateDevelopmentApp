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
    public class ConstructionScheduleViewModel : LoadAllViewModel<ConstructionScheduleEntityForView>
    {
        #region Properties
        private ConstructionScheduleEntityForView _selectedItem;
        public ConstructionScheduleEntityForView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        private string _filterBuildingName;
        public string FilterBuildingName
        {
            get => _filterBuildingName;
            set
            {
                _filterBuildingName = value;
                OnPropertyChanged(() => FilterBuildingName);
                ApplyFiltersAsync();
            }
        }

        private string _filterBuildingNumber;
        public string FilterBuildingNumber
        {
            get => _filterBuildingNumber;
            set
            {
                _filterBuildingNumber = value;
                OnPropertyChanged(() => FilterBuildingNumber);
                ApplyFiltersAsync();
            }
        }

        private DateTime _filterStartDate;
        public DateTime FilterStartDate
        {
            get => _filterStartDate;
            set
            {
                _filterStartDate = value;
                OnPropertyChanged(() => FilterStartDate);
                ApplyFiltersAsync();
            }
        }

        private DateTime _filterEndDate;
        public DateTime FilterEndDate
        {
            get => _filterEndDate;
            set
            {
                _filterEndDate = value;
                OnPropertyChanged(() => FilterEndDate);
                ApplyFiltersAsync();
            }
        }

        private ObservableCollection<ConstructionScheduleEntityForView> _filteredList;
        public ObservableCollection<ConstructionScheduleEntityForView> FilteredList
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
        public RealyCommand OpenScheduleNewConstructionCommand { get; set; }
        #endregion
        public event Action ScheduleNewConstructionRequested;
        public RealyCommand DeleteSelectedCommand { get; set; }
        public RealyCommand ApplyFiltersCommand { get; set; }



        #region Constructor
        public ConstructionScheduleViewModel()
            :base()
        {

            OpenScheduleNewConstructionCommand = new RealyCommand(o =>
            {
                ScheduleNewConstructionRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
            ApplyFiltersCommand = new RealyCommand(_ => ApplyFiltersAsync());
        }
        #endregion

        #region Helpers
        public override async Task LoadAsync()
        {
                var query = from c in realEstateEntities.ConstructionSchedule
                       join b in realEstateEntities.Buildings on c.BuildingId equals b.BuildingID
                       join p in realEstateEntities.Projects on c.ProjectID equals p.ProjectID
                       where c.ProjectID==p.ProjectID 
                       select new ConstructionScheduleEntityForView
                       {
                           ScheduleID = c.ScheduleID,
                           BuildingId = b.BuildingID,
                           BuildingName = b.BuildingName,
                           BuildingNumber = b.BuildingNumber,
                           Floors = b.Floors,
                           Address = p.Location,
                           NumberOfApartments = realEstateEntities.Apartments.Count(a => a.BuildingID == b.BuildingID),
                           StartDate = c.StartDate,
                           EndDate = c.EndDate,
                           Status = c.Status,
                       };

            


            var result = await query.ToListAsync();
            List = new ObservableCollection<ConstructionScheduleEntityForView>(result);
        }

        public override Task ApplyFiltersAsync()
        {
            FilteredList = new ObservableCollection<ConstructionScheduleEntityForView>(
                List.Where(c =>
                    (string.IsNullOrEmpty(FilterBuildingName) || c.BuildingName.Contains(FilterBuildingName)) &&
                    (string.IsNullOrEmpty(FilterBuildingNumber) || c.BuildingNumber.Contains(FilterBuildingNumber)) &&
                    (FilterStartDate == DateTime.MinValue || c.StartDate >= FilterStartDate) &&
                    (FilterEndDate == DateTime.MinValue || c.EndDate <= FilterEndDate)
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
            if (SelectedItem is ConstructionScheduleEntityForView selectedConstruction)
            {
                var modal = new DeleteConstructionScheduleModalView();
                DeleteConstructionScheduleModalViewModel deleteConstructionModalViewModel = new DeleteConstructionScheduleModalViewModel(
                                            realEstateEntities.ConstructionSchedule.First(c => c.ScheduleID == selectedConstruction.ScheduleID));
                deleteConstructionModalViewModel.RequestClose += (obj, sender) =>
                {
                    modal.Close();
                };
                modal.DataContext = deleteConstructionModalViewModel;
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
