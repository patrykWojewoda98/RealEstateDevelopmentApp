using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.ViewModels;
using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        private RealEstateEntities estateEntities;
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
            LogInCommand = new RealyCommand(o => LogIn());
            var loginchecker = new LoginChecker();
            
        }
        #endregion

        #region Methods
        private void LogIn()
        {
            Console.WriteLine("Loguje! ");
            var employee = estateEntities.Employees.FirstOrDefault(e => e.LastName == _login && e.Password == _password);

            if (employee!=null)
            {
                LoginSuccessful?.Invoke(true); // Powiadomienie, że logowanie się powiodło
            }
            else
            {
                LoginSuccessful?.Invoke(false); // Logowanie nie powiodło się
            }
        }

        
        #endregion
    }
}
