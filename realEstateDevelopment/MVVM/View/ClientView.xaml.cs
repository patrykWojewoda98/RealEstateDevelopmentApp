using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class ClientView : UserControl
    {
        public ClientView()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ClientForView selectedClient)
            {
                if (DataContext is ClientViewModel viewModel)
                {
                    viewModel.SelectedItem = selectedClient;
                    
                    viewModel.Update(viewModel.SelectedItem.Id);
                }
            }
        }



        private void DeleteCommand(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ClientViewModel viewModel && viewModel.SelectedItem != null)
            {

                // Wywołaj polecenie usunięcia.
                viewModel.DeleteSelectedCommand.Execute(null);
            }
        }
    }
}
