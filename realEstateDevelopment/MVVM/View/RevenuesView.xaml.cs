using GalaSoft.MvvmLight.Messaging;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class RevenuesView : UserControl
    {
        public RevenuesView()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is RevenuesEntityForView selectedOperation)
            {
                if (DataContext is RevenuesViewModel viewModel)
                {
                    // Ustaw ID w ViewModelu na podstawie wybranego obiektu
                    viewModel.SelectedItem = selectedOperation;
                    var updateMessage = new UpdateMessage("RevenueUpdate", selectedOperation.Id);
                    Messenger.Default.Send(updateMessage);

                }
            }
        }
    }
}
