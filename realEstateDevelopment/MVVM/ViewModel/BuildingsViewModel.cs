using realEstateDevelopment.MVVM.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class BuildingsViewModel : LoadAllViewModel<Buildings>
    {
        #region Constructor
        public BuildingsViewModel()
            :base() 
        {
            base.DisplayName = "Budynki";
        }
        #endregion

        #region Helpers
        public override void load()
        {
            List = new ObservableCollection<Buildings>(realEstateEntities.Buildings.ToList());
        }
        #endregion
    }
}
