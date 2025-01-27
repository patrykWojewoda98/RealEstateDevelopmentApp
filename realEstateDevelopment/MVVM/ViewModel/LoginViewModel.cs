using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.ViewModels;
using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using System;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        private string _password;
        private string _login;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(() => Password);
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(() => Login);
            }
        }
        #endregion

        #region Commands
        public RealyCommand LogInCommand { get; set; }
        
        #endregion

        #region Events
        public event Action<bool> LoginSuccessful;
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            LogInCommand = new RealyCommand(o => TryLogIn());
            

        }
        #endregion

        #region Methods
        

        private void TryLogIn()
        {
            Console.WriteLine("Wysłałem prośbę o logowanie");
            var message = new LogInMessage(Login, Password);
            Messenger.Default.Send(message);
        }

        

        #endregion
    }
}
