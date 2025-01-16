using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class MaintenanceRequestsView : UserControl
    {
        public MaintenanceRequestsView()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is MaintenanceRequestsEntityForView selectedMaintenanceRequest)
            {
                if (DataContext is MaintenanceRequestsViewModel viewModel)
                {
                    // Ustaw ID w ViewModelu na podstawie wybranego obiektu
                    viewModel.SelectedItem = selectedMaintenanceRequest.Id;

                    // Wywołaj Update
                    viewModel.Update(viewModel.SelectedItem);
                }
            }
        }
    }
}
