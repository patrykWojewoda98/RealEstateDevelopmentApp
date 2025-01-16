using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class PurchasesView : UserControl
    {
        public PurchasesView()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is PurchasesEntityForView selectedPurchase)
            {
                if (DataContext is PurchasesViewModel viewModel)
                {
                    // Ustaw ID w ViewModelu na podstawie wybranego obiektu
                    viewModel.SelectedItem = selectedPurchase.PurchaseID;

                    // Wywołaj Update
                    viewModel.Update(viewModel.SelectedItem);
                }
            }
        }
    }
}
