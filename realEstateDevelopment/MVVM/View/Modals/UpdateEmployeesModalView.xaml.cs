using realEstateDevelopment.MVVM.ViewModel.Modals;
using System.Windows;
using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View.Modals
{
    public partial class UpdateEmployeesModalView : Window
    {
        public UpdateEmployeesModalView()
        {
            InitializeComponent();
        }

        private void OryginalPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = DataContext as UpdateEmployeesModalViewModel;
                if (viewModel != null)
                {
                    viewModel.Password = passwordBox.Password;
                }
            }
        }
    }
}
