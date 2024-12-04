using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ApartmentsViewModel : LoadAllViewModel<Apartments>
    {
        #region Commands
        public RealyCommand OpenAddNewApartmentCommand { get; set; }
        #endregion
        public event Action AddNewApartmentRequested;
        #region
        public ApartmentsViewModel()
            : base()
        {
            OpenAddNewApartmentCommand = new RealyCommand(o =>
            {
                AddNewApartmentRequested?.Invoke();
            });
        }
        #endregion
        #region Helpers
        public override async Task LoadAsync()
        {
            var apartments = await Task.Run(() => realEstateEntities.Apartments.ToList());
            List = new ObservableCollection<Apartments>(apartments);
        }
        #endregion
    }
}