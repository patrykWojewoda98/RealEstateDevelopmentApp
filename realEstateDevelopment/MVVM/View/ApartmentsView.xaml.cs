
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class ApartmentsView : UserControl
    {
        public ApartmentsView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ApartmentsEntitiesForView selectedApartment)
            {
                if (DataContext is ApartmentsViewModel viewModel)
                {
                    viewModel.SelectedItem = selectedApartment; // Przekazanie całego obiektu.
                    Console.WriteLine("Wybrano mieszkanie: " + viewModel.SelectedItem.ApartmentID);

                    viewModel.Update(viewModel.SelectedItem.ApartmentID); // Wywołanie Update.
                }
            }
        }

        private void DeleteCommand(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ApartmentsViewModel viewModel && viewModel.SelectedItem != null)
            {
                Console.WriteLine("Usuwanie mieszkania o ID: " + viewModel.SelectedItem.ApartmentID);

                // Wywołaj polecenie usunięcia.
                viewModel.DeleteSelectedCommand.Execute(null);
            }
        }

    }
}
