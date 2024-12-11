using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View
{
    public partial class AddNewReservationsView : UserControl
    {
        public AddNewReservationsView()
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
