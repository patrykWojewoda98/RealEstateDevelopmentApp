using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class HistoryOfChangesViewModel : LoadAllViewModel<HistoryOfChangesEntityForView>
    {
        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }
        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from h in realEstateEntities.HistoryOfChanges
                        join e in realEstateEntities.Employees on h.EmployeeId equals e.EmployeeID
                        select new HistoryOfChangesEntityForView
                        {
                            Id = h.IdOfChange,
                            EmployeeName = e.FirstName,
                            EmployeeSurname = e.LastName,
                            Operation = h.Operation,
                            DateAndTimeOfChange = h.DateAndTimeOfChange
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<HistoryOfChangesEntityForView>(result);
        }
        #endregion
    }
}
