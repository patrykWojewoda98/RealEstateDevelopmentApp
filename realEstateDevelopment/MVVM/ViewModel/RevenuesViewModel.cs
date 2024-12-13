using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class RevenuesViewModel : LoadAllViewModel<RevenuesEntityForView>
    {

        #region Commands
        public RealyCommand OpenAddNewRevenueCommand { get; set; }
        #endregion

        #region Events
        public event Action AddNewRevenueRequested;
        #endregion

        public RevenuesViewModel()
            : base()
        {
            OpenAddNewRevenueCommand = new RealyCommand(o =>
            {
                AddNewRevenueRequested?.Invoke();
            });
        }

        #region Helpers
        public override async Task LoadAsync()
        {
            var query = from r in realEstateEntities.Revenues
                        join p in realEstateEntities.Projects on r.ProjectID equals p.ProjectID
                        select new RevenuesEntityForView
                        {
                            Id = r.RevenueID,
                            ProjectName = p.ProjectName,
                            Address = p.Location,
                            RevenueType = r.RevenueType,
                            RevenueAmount = r.Amount,
                            RevenueDate = r.RevenueDate
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<RevenuesEntityForView>(result);
        }
        #endregion
    }
}
