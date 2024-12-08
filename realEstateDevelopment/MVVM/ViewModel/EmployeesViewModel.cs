using realEstateDevelopment.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading.Tasks;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.Model.Entities;
using System.Data.Entity;

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
            var query = from e in realEstateEntities.Employees
                        join ep in realEstateEntities.EmployeeProjects on e.EmployeeID equals ep.EmployeeID
                        join p in realEstateEntities.Projects on ep.ProjectID equals p.ProjectID
                        group p by new
                        {
                            e.EmployeeID,
                            e.FirstName,
                            e.LastName,
                            e.Position,
                            e.Department,
                            e.Salary
                        } into g
                        select new EmployeesEntityForView
                        {
                            EmployeeId = g.Key.EmployeeID,
                            FirstName = g.Key.FirstName,
                            LastName = g.Key.LastName,
                            Position = g.Key.Position,
                            Department = g.Key.Department,
                            Salary = g.Key.Salary,
                            Projects = g.Select(proj => proj.ProjectName).ToList(),
                            SelectedProject = g.Select(proj => proj.ProjectName).FirstOrDefault()
                        };

            var result = await query.ToListAsync();
            List = new ObservableCollection<EmployeesEntityForView>(result);
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
