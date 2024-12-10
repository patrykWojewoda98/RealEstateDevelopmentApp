using System;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class ConstructionScheduleEntityForView
    {
        #region Properties
        public int ScheduleID { get; set; }
        public string Address {  get; set; }
        public int BuildingId {  get; set; }
        public string BuildingName {  get; set; }
        public string BuildingNumber {  get; set; }
        public int Floors {  get; set; }
        public int NumberOfApartments {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        #endregion

    }
}
