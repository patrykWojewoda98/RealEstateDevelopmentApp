using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class RevenuesViewModel : LoadAllViewModel<RevenuesEntityForView>
    {


        #region Helpers
        public override async Task LoadAsync()
        {
            var revenues = realEstateEntities.Revenues;
            var project = realEstateEntities.Projects;

            List = new ObservableCollection<RevenuesEntityForView>(revenues.Join(project,
                                                                                            r => r.ProjectID,
                                                                                            p => p.ProjectID,
                                                                                            (r, p) => new RevenuesEntityForView
                                                                                            {
                                                                                              Id = r.RevenueID,
                                                                                              ProjectName = p.ProjectName,
                                                                                              Address = p.Location,
                                                                                              RevenueType = r.RevenueType,
                                                                                              RevenueAmount = r.Amount,
                                                                                              RevenueDate = r.RevenueDate
                                                                                            })
                .ToList());



        }
        #endregion
    }
}
