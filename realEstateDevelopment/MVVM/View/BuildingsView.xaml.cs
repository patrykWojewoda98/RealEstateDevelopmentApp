using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System;
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
                    viewModel.SelectedItem = selectedBuilding;
                    viewModel.Update(viewModel.SelectedItem.BuildingID);
                }
            }

        }

        private void DeleteCommand(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is BuildingsViewModel viewModel && viewModel.SelectedItem != null)
            {
                Console.WriteLine("Usuwanie mieszkania o ID: " + viewModel.SelectedItem.BuildingID);

                // Wywołaj polecenie usunięcia.
                viewModel.DeleteSelectedCommand.Execute(null);
            }
        }
    }
}