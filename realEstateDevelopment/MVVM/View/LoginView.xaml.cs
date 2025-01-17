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
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            Messenger.Default.Register<LoginSucceedMessage>(this, OnLoginSucceeded);
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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

        

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Wysłałem prośbę o logowanie");
            var viewModel = DataContext as LoginViewModel;
            var message = new LogInMessage(viewModel.Login, viewModel.Password);
            Messenger.Default.Send(message);
        }

        private void OnLoginSucceeded(LoginSucceedMessage message)
        {
            // Zamknij okno logowania
            var mainWindow = new MainWindow();
            mainWindow.Show();
            WindowState = WindowState.Minimized;
            Console.WriteLine($"Zalogowano użytkownika: {message.employee.FirstName} {message.employee.LastName}");
            
        }

        
    }
}
