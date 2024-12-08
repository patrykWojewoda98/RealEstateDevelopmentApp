using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ExpensesViewModel : LoadAllViewModel<ExpensesEntityForView>
    {
        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from e in realEstateEntities.Expenses
                        join p in realEstateEntities.Projects on e.ProjectID equals p.ProjectID
                        select new ExpensesEntityForView
                        {
                            Id = e.ExpenseID,
                            ProjectName = p.ProjectName,
                            Address = p.Location,
                            ExpenseType = e.ExpenseType,
                            ExpenseAmount = e.Amount,
                            ExpenseDate = e.ExpenseDate
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<ExpensesEntityForView>(result);
        }
        #endregion
    }
}
