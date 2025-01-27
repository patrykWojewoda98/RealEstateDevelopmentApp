using GalaSoft.MvvmLight.Messaging;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View
{
    public partial class LoginView : Window
    {
        LoginChecker loginchecker;
        public LoginView()
        {
            DataContext = new LoginViewModel();
            loginchecker = new LoginChecker();
            Messenger.Default.Register<LoginSucceedMessage>(this, OnLoginSucceeded);
            InitializeComponent();
        }

       
        

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = DataContext as LoginViewModel;
                if (viewModel != null)
                {
                    viewModel.Password = passwordBox.Password;
                }
            }
        }

        

        

        private void OnLoginSucceeded(LoginSucceedMessage message)
        {
            // Zamknij okno logowan
            this.Hide();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            WindowState = WindowState.Minimized;
            Console.WriteLine($"Zalogowano użytkownika: {message.employee.FirstName} {message.employee.LastName}");
            
        }

        
    }
}
