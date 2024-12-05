using realEstateDevelopment.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading.Tasks;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.Model.Entities;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class EmployeesViewModel : LoadAllViewModel<EmployeesEntityForView>
    {
        #region Commands
        public RealyCommand OpenAddNewEmployeeCommand { get; set; }
        #endregion
        public event Action AddNewEmployeeRequested;
        #region
        public EmployeesViewModel()
            : base()
        {
            OpenAddNewEmployeeCommand = new RealyCommand(o =>
            {
                AddNewEmployeeRequested?.Invoke();
            });
        }
        #endregion
        #region Helpers
        public override async Task LoadAsync()
        {
            var employees = realEstateEntities.Employees;
            var employeeProjects = realEstateEntities.EmployeeProjects;
            var projects = realEstateEntities.Projects;
            List = new ObservableCollection<EmployeesEntityForView>(employees.Join(employeeProjects,
                                                                                    e => e.EmployeeID,
                                                                                    ep => ep.EmployeeID,
                                                                                    (e, ep) => new EmployeesEntityForView
                                                                                    {
                                                                                        EmployeeId = e.EmployeeID,
                                                                                        FirstName = e.FirstName,
                                                                                        LastName = e.LastName,
                                                                                        Position = e.Position,
                                                                                        Department = e.Department,
                                                                                        Salary = e.Salary,
                                                                                        Projects = employeeProjects.Where(epr => ep.EmployeeID == e.EmployeeID)
                                                                                                                   .Join(projects,
                                                                                                                         epr => ep.ProjectID,
                                                                                                                         p => p.ProjectID,
                                                                                                                         (epr, p) => p.ProjectName).ToList(),
                                                                                        SelectedProject = employeeProjects.Where(epr => ep.EmployeeID == e.EmployeeID)
                                                                                                                   .Join(projects,
                                                                                                                         epr => ep.ProjectID,
                                                                                                                         p => p.ProjectID,
                                                                                                                         (epr, p) => p.ProjectName).FirstOrDefault()
                                                                                    }));
            foreach (var employee in List)
            {
                for (int i = 0; i <= 20; i++) {
                    Console.WriteLine("**********");
                        }
                Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}");
                Console.WriteLine("Projects:");
                foreach (var project in employee.Projects)
                {
                    Console.WriteLine($"- {project}");
                }
                Console.WriteLine($"Selected Project: {employee.SelectedProject}");
            }
        }
        #endregion
    }
}
