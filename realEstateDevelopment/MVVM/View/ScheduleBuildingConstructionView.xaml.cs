using System.Windows;
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

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                // Automatyczne otwarcie kalendarza
                datePicker.IsDropDownOpen = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
