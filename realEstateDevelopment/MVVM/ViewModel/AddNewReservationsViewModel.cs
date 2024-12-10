using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewReservationsViewModel : BaseDatabaseAdder<Reservations>
    {
        private string _clientName;
        private Apartments _selectedApartment;
        public ObservableCollection<AddNewReservationEntitiesForView> AvailableApartments { get; set; }

        public int ClientID
        {
            get => item.ClientID;  // Poprawiono getter, by zwracał wartość z 'item'
            set
            {
                item.ClientID = value;  // Przypisanie do item.ClientID
                OnPropertyChanged(() => ClientID);
            }
        }

        public Apartments SelectedApartment
        {
            get => _selectedApartment;
            set
            {
                _selectedApartment = value;
            }
        }

        #region Constructor

        public AddNewReservationsViewModel()
        {
            var query = from a in estateEntities.Apartments
                        join b in estateEntities.Buildings on a.BuildingID equals b.BuildingID
                        select new AddNewReservationEntitiesForView
                        {
                            ApartmentNumber = a.ApartmentNumber,
                            BuildingNumber = b.BuildingNumber,
                        };

            
            AvailableApartments = new ObservableCollection<AddNewReservationEntitiesForView>(
                query.ToList() 
            );
        }

        #endregion

        public override void Save()
        {
            // Implementacja zapisu
            throw new System.NotImplementedException();
        }
    }
}
