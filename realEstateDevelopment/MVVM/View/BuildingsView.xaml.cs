using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class BuildingsView : UserControl
    {
        public BuildingsView()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is BuildingsEntityForView selectedBuilding)
            {
                if(DataContext is BuildingsViewModel viewModel)
                {
                    viewModel.SelectedItem = selectedBuilding.BuildingID;
                    viewModel.Update(viewModel.SelectedItem);
                }
            }
        }
    }
}