using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View
{
    public partial class AddNewRevenueView : UserControl
    {
        public AddNewRevenueView()
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
