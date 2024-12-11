using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View
{

    public partial class AddNewMaintenanceRequestsView : UserControl
    {
        public AddNewMaintenanceRequestsView()
        {
            InitializeComponent();
        }
        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboItem = sender as ComboBox;
            string name = selectedComboItem.SelectedItem as string;

        }
    }
}
