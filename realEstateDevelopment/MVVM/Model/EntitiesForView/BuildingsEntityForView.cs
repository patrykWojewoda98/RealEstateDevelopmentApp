namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class BuildingsEntityForView
    {
        #region Properties
        public int BuildingID { get; set; }

        public string BuildingNumber { get; set; }

        public int ProjectID { get; set; }
        public string BuildingName { get; set; }
        public int Floors { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        #endregion
    }
}
