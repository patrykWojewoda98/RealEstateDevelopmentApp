using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class SuppliersView : UserControl
    {
        public SuppliersView()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is SuppliersEntityForView selectedSupplier)
            {
                if (DataContext is SuppliersViewModel viewModel)
                {
                    // Ustaw ID w ViewModelu na podstawie wybranego obiektu
                    viewModel.SelectedItem = selectedSupplier;
                    // Wywołaj Update
                    viewModel.Update(viewModel.SelectedItem.SupplierID);
                }
            }
        }
    }
}
