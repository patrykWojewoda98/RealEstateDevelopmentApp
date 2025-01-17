using GalaSoft.MvvmLight.Messaging;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace realEstateDevelopment.Helper
{
    public class LoginChecker
    {

        protected readonly RealEstateEntities realEstateEntities;

        public LoginChecker()
        {
            realEstateEntities = new RealEstateEntities();
            Messenger.Default.Register<LogInMessage>(this, Open);
        }

        private void Open(LogInMessage updateMessage)
        {
            Console.WriteLine("Przetwarzam dane");
            var employee = realEstateEntities.Employees.FirstOrDefault(e=>e.LastName == updateMessage.Login && e.Password == updateMessage.Password);
            if (employee != null)
            {
                var mainViewModel = new MainViewModel();
                Messenger.Default.Send(new LoginSucceedMessage(new EmployeesEntityForView
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Position = employee.Position,
                    EmployeeId = employee.EmployeeID,
                    Department = employee.Department
                }
                    ));
            }
        }
    }
}
