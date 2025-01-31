using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class DeleteConstructionScheduleModalViewModel : BaseDataDeleter<ConstructionSchedule>
    {
        #region Properties

        private RealEstateEntities estateEntities;

        public int ScheduleID
        {
            get => item.ScheduleID;
            set
            {
                item.ScheduleID = value;
                OnPropertyChanged(() => ScheduleID);
            }
        }

        public string BuildingName
        {
            get => estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == item.BuildingId).BuildingName;
        }

        public string BuildingNumber
        {
            get => estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == item.BuildingId).BuildingNumber;
        }

        public int Floors
        {
            get => estateEntities.Buildings.FirstOrDefault(b => b.BuildingID == item.ProjectID)?.Floors ?? 0;
        }

        public int NumberOfApartments
        {
            get => estateEntities.Apartments.Count(a => a.BuildingID == item.ProjectID);
        }

        public string Address
        {
            get => estateEntities.Projects.FirstOrDefault(p => p.ProjectID == item.ProjectID)?.Location;
        }

        public DateTime StartDate
        {
            get => item.StartDate;
            set
            {
                item.StartDate = value;
                OnPropertyChanged(() => StartDate);
            }
        }


        public string Status
        {
            get => item.Status;
            set
            {
                item.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        #endregion

        #region Commands
        public RealyCommand ConfirmDeleteCommand { get; set; }
        public RealyCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DeleteConstructionScheduleModalViewModel(ConstructionSchedule schedule)
            : base()
        {
            item = schedule;
            estateEntities = new RealEstateEntities();
            ConfirmDeleteCommand = new RealyCommand(ExecuteDelete);
            CancelCommand = new RealyCommand(CancelDelete);
        }
        #endregion

        #region Helpers
        public override void Delete()
        {
            var existingItem = estateEntities.ConstructionSchedule.FirstOrDefault(s => s.ScheduleID == item.ScheduleID);
            if (existingItem != null)
            {
                estateEntities.ConstructionSchedule.Remove(existingItem);
                estateEntities.SaveChanges();
                SaveHistoryOfChanges();
            }
            else
            {
                var errorModal = new ErrorModalView("Nie znaleziono harmonogramu do usunięcia.");
                errorModal.ShowDialog();
            }
        }

        private void ExecuteDelete(object parameter)
        {
            try
            {
                Delete();
                base.OnRequestClose();
            }
            catch (Exception ex)
            {
                var errorModal = new ErrorModalView($"Wystąpił błąd: {ex.Message}");
                errorModal.ShowDialog();
            }
        }

        private void CancelDelete(object parameter)
        {
            base.OnRequestClose();
        }
        #endregion
    }
}
