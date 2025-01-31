using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    public partial class ProjectsView : UserControl
    {
        public ProjectsView()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ProjectEntityForView selectedProject)
            {
                if (DataContext is ProjectsViewModel viewModel)
                {
                    // Ustaw ID w ViewModelu na podstawie wybranego obiektu
                    viewModel.SelectedItem = selectedProject;
                    Console.WriteLine("Wybrane ID: " + viewModel.SelectedItem);

                    // Wywołaj Update
                    viewModel.Update(viewModel.SelectedItem.ProjectId);
                }
            }
        }
    }
}
