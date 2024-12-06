using realEstateDevelopment.Core;
using realEstateDevelopment.MVVM.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class FinancesViewModel : ObservableObject
    {
        #region Commands
        public RealyCommand RevenueViewCommand { get; set; }

        #endregion


        #region Properties
        public RevenuesViewModel RevenuesVM { get; set; }
        

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public FinancesViewModel()
        {
            RevenuesVM = new RevenuesViewModel();
            

            RevenueViewCommand = new RealyCommand(o =>
            {
                CurrentView = RevenuesVM;
            });

            

            
        }
        #endregion

    }
}
