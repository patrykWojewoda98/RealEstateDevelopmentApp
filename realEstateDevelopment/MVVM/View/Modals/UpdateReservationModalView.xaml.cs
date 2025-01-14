using System.Windows;
using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View.Modals
{
    
    public partial class UpdateReservationModalView : Window
    {
        public UpdateReservationModalView()
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
