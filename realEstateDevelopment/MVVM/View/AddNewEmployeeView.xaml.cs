using realEstateDevelopment.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View
{

    public partial class AddNewEmployeeView : UserControl
    {
        public AddNewEmployeeView()
        {
            InitializeComponent();
        }
        private void OryginalPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = DataContext as AddNewEmployeeViewModel;
                if (viewModel != null)
                {
                    viewModel.OriginalPassword = passwordBox.Password;
                }
            }
        }
        private void PepeatedPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = DataContext as AddNewEmployeeViewModel;
                if (viewModel != null)
                {
                    viewModel.RepeatedPassword = passwordBox.Password;
                }
            }
        }
    }
}
