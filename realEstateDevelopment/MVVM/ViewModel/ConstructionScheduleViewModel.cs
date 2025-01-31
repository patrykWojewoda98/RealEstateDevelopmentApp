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
        #endregion

        #region Commands
        public RealyCommand OpenScheduleNewConstructionCommand { get; set; }
        #endregion
        public event Action ScheduleNewConstructionRequested;
        public RealyCommand DeleteSelectedCommand { get; set; }



        #region Constructor
        public ConstructionScheduleViewModel()
            :base()
        {

            OpenScheduleNewConstructionCommand = new RealyCommand(o =>
            {
                ScheduleNewConstructionRequested?.Invoke();
            });
            DeleteSelectedCommand = new RealyCommand(ExecuteDeleteSelected, CanExecuteDeleteSelected);
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
            throw new NotImplementedException();
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
