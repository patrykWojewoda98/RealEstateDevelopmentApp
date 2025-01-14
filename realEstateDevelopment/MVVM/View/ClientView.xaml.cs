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
                    viewModel.SelectedItem = selectedClient.Id;
                    Console.WriteLine("Wybrane ID: " + viewModel.SelectedItem);
                    viewModel.Update(viewModel.SelectedItem);
                }
            }
        }
    }
}
