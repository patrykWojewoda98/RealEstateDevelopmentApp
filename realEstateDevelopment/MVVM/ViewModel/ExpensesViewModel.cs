using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ExpensesViewModel : LoadAllViewModel<ExpensesEntityForView>
    {
        #region Helpers
        public override async Task LoadAsync()
        {
            var expenses = realEstateEntities.Expenses;
            var project = realEstateEntities.Projects;

            List = new ObservableCollection<ExpensesEntityForView>(expenses.Join(project,
                                                                                            e => e.ProjectID,
                                                                                            p => p.ProjectID,
                                                                                            (e, p) => new ExpensesEntityForView
                                                                                            {
                                                                                                Id = e.ExpenseID,
                                                                                                ProjectName = p.ProjectName,
                                                                                                Address = p.Location,
                                                                                                ExpenseType = e.ExpenseType,
                                                                                                ExpenseAmount = e.Amount,
                                                                                                ExpenseDate = e.ExpenseDate
                                                                                            })
                .ToList());



        }
        #endregion
    }
}
