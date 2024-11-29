using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewBuildingViewModel : BaseDatabaseAdder<Buildings>
    {
        #region Propeties
        public int BuildingID
        {
            get
            {
                return item.BuildingID;
            }
            set
            {
                item.BuildingID = value;
                OnPropertyChanged(() => BuildingID);
            }
        }

        public int ProjectID
        {
            get
            {
                return item.ProjectID;
            }
            set
            {
                item.ProjectID = value;
                OnPropertyChanged(() => ProjectID);
            }
        }
        public string BuildingName
        {
            get
            {
                return item.BuildingName;
            }
            set
            {
                item.BuildingName = value;
                OnPropertyChanged(() => BuildingName);
            }
        }
        public int Floors
        {
            get
            {
                return item.Floors;
            }
            set
            {
                item.Floors = value;
                OnPropertyChanged(() => Floors);
            }
        }
        public string Status
        {
            get
            {
                return item.Status;
            }
            set
            {
                item.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        #endregion
        #region Constructor
        public AddNewBuildingViewModel()
            : base()
        {
            item = new Buildings();
        }


        #endregion

        #region Helpers

        public override void Save()
        {
            estateEntities.Buildings.Add(item);
            estateEntities.SaveChanges();
        }
        #endregion
    }
}
