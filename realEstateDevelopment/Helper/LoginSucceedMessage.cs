using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.Helper
{
    public class LoginSucceedMessage
    {
        public EmployeesEntityForView employee;

        public LoginSucceedMessage(EmployeesEntityForView e)
        {
            employee = e;
        }
    }
}
