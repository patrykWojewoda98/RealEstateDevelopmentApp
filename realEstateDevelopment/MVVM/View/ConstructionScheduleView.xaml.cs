using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class ConstructionScheduleView : UserControl
    {
        public ConstructionScheduleView()
        {
            InitializeComponent();

        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ConstructionScheduleEntityForView selectedconstructionSchedule)
            {
                if (DataContext is ConstructionScheduleViewModel viewModel)
                {
                    // Ustaw ID w ViewModelu na podstawie wybranego obiektu
                    viewModel.SelectedItem = selectedconstructionSchedule.ScheduleID;
                    Console.WriteLine("Wybrane ID: " + viewModel.SelectedItem);

                    // Wywołaj Update
                    viewModel.Update(viewModel.SelectedItem);
                }
            }
        }
    }
}
