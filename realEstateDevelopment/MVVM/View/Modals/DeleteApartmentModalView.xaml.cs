using realEstateDevelopment.MVVM.ViewModel;
using System.Windows;

namespace realEstateDevelopment.MVVM.View.Modals
{
    /// <summary>
    /// Interaction logic for DeleteApartmentModalView.xaml
    /// </summary>
    public partial class DeleteApartmentModalView : Window
    {
        public DeleteApartmentModalView()
        {
            InitializeComponent();
            if (DataContext is WorkspaceViewModel viewModel)
            {
                viewModel.RequestClose += (s, e) => this.Close();
            }
        }
    }
}
