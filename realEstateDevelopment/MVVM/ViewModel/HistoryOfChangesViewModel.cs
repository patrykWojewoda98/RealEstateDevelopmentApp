using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class HistoryOfChangesViewModel : LoadAllViewModel<HistoryOfChangesEntityForView>
    {
        #region Helpers
        public override async Task LoadAsync()
        {
            var historyOfChanges = realEstateEntities.HistoryOfChanges;
            var employees = realEstateEntities.Employees;

            List = new ObservableCollection<HistoryOfChangesEntityForView>(historyOfChanges.Join(employees,
                                                                                            h => h.EmployeeId,
                                                                                            e => e.EmployeeID,
                                                                                            (h, e) => new HistoryOfChangesEntityForView
                                                                                            {
                                                                                                Id = h.IdOfChange,
                                                                                                EmployeeName = e.FirstName,
                                                                                                EmployeeSurname = e.LastName,
                                                                                                Operation = h.Operation,
                                                                                                DateAndTimeOfChange = h.DateAndTimeOfChange
                                                                                            })
                .ToList());



        }
        #endregion
    }
}
