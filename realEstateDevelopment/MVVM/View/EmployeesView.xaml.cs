using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class EmployeesView : UserControl
    {
        public EmployeesView()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is EmployeesEntityForView selectedMaterial)
            {
                if (DataContext is EmployeesViewModel viewModel)
                {
                    viewModel.SelectedItem = selectedMaterial;

                    viewModel.Update(viewModel.SelectedItem.EmployeeId);
                }
            }
        }
    }
}
