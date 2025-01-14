using System.Windows;
using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View.Modals
{
    public partial class UpdateMaterialModalView : Window
    {
        public UpdateMaterialModalView()
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
