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
    public class AllCashOperationsViewModel : LoadAllViewModel<AllCashOperationsEntityForView>
    {
        public AllCashOperationsViewModel()
            :base()
        {
            
        }

        public override Task ApplyFiltersAsync()
        {
            throw new NotImplementedException();
        }

        public override async Task LoadAsync()
        {
            var query = from r in realEstateEntities.Revenues
                        join p in realEstateEntities.Projects on r.ProjectID equals p.ProjectID
                        select new AllCashOperationsEntityForView
                        {
                            ProjectName = p.ProjectName,
                            Type = "Przychód",
                            Category = r.RevenueType,
                            Amount = r.Amount,
                            Date = r.RevenueDate
                        };

            var result = await query.ToListAsync();
            
            var query2 = from e in realEstateEntities.Expenses
                        join p in realEstateEntities.Projects on e.ProjectID equals p.ProjectID
                        select new AllCashOperationsEntityForView
                        {
                            ProjectName = p.ProjectName,
                            Type = "Koszt",
                            Category = e.ExpenseType,
                            Amount = e.Amount,
                            Date = e.ExpenseDate
                        };

            var result2 = await query2.ToListAsync();

            var finalResult = new List<AllCashOperationsEntityForView>();
            finalResult.AddRange(result);
            finalResult.AddRange(result2);
            finalResult = finalResult.OrderBy(x => x.Date).ToList();


            List = new ObservableCollection<AllCashOperationsEntityForView>(finalResult);
        }
    }
}
