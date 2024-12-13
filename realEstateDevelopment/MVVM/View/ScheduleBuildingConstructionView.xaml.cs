using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View
{
    public partial class ScheduleBuildingConstructionView : UserControl
    {
        public ScheduleBuildingConstructionView()
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
