using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class SalesView : UserControl
    {
        public SalesView()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is SalesEntityForView selectedSale)
            {
                if (DataContext is SalesViewModel viewModel)
                {
                    // Ustaw ID w ViewModelu na podstawie wybranego obiektu
                    viewModel.SelectedItem = selectedSale.Id;

                    // Wywołaj Update
                    viewModel.Update(viewModel.SelectedItem);
                }
            }
        }
    }
}
