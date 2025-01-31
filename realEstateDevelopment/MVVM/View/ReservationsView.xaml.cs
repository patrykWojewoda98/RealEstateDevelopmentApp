using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class ReservationsView : UserControl
    {
        public ReservationsView()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ReservationsEntityForView selectedMaterial)
            {
                if (DataContext is ReservationsViewModel viewModel)
                {
                    // Ustaw ID w ViewModelu na podstawie wybranego obiektu
                    viewModel.SelectedItem = selectedMaterial;

                    // Wywołaj Update
                    viewModel.Update(viewModel.SelectedItem.Id);
                }
            }
        }
    }
}
