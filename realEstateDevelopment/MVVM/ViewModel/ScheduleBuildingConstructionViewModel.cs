using realEstateDevelopment.MVVM.Model.Entities;
using realEstateDevelopment.Helper;
using System.Collections.Generic;
using System.Linq;
using realEstateDevelopment.MVVM.View.Modals;
using System;
using realEstateDevelopment.MVVM.ViewModel.Modals;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using System.Collections.ObjectModel;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class ScheduleBuildingConstructionViewModel : BaseDatabaseAdder<ConstructionSchedule>
    {
        #region Properties

        private ProjectEntityForView _selectedProject;

        public ObservableCollection<ProjectEntityForView> AvailableProject { get; set; }


        public ProjectEntityForView SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                if (_selectedProject != null)
                {
                    item.ProjectID = _selectedProject.ProjectId;
                }
                OnPropertyChanged(() => SelectedProject);
            }
        }
        

        public string ConstructionPhase
        {
            get => item.ConstructionPhase;
            set
            {
                item.ConstructionPhase = "Zaplanowano";
            }
        }

        public DateTime StartDate
        {
            get => item.StartDate;
            set
            {
                item.StartDate = value;
                OnPropertyChanged(() => StartDate);
            }
        }

        public DateTime? EndDate
        {
            get => item.EndDate;
            set
            {
                item.EndDate = value;
                OnPropertyChanged(() => EndDate);
            }
        }

        public string BuildingName { get; set; }
        public string BuildingNumber { get; set; }
        public int Floors { get; set; }
        public int NumberOfApartments { get; set; }
        #endregion

        #region Constructor
        public ScheduleBuildingConstructionViewModel()
            : base()
        {
            item = new ConstructionSchedule();

            AvailableProject = new ObservableCollection<ProjectEntityForView>();
            LoadProjects();

            SelectedProject = AvailableProject.FirstOrDefault();
        }
        #endregion

        #region Validation
        public void Validate()
        {
            isDataCorrect = true;
            var errors = new List<string>();
            var db = new RealEstateEntities();

           

            if (StartDate > EndDate)
            {
                errors.Add("Data rozpoczęcia nie może być późniejsza niż data zakończenia.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(BuildingName))
            {
                errors.Add("Nazwa budynku jest wymagana.");
                isDataCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(BuildingNumber))
            {
                errors.Add("Numer budynku jest wymagany.");
                isDataCorrect = false;
            }
            
            if (db.Buildings.Any(b => b.BuildingNumber == BuildingNumber && b.ProjectID == SelectedProject.ProjectId))
            {
                errors.Add("Budynek o podanym numerze i ID projektu już istnieje.");
                isDataCorrect = false;
            }

            if (Floors < 0)
            {
                errors.Add("Liczba pięter musi być większa bądź równa 0.");
                isDataCorrect = false;
            }

            if (NumberOfApartments <= 0)
            {
                errors.Add("Liczba mieszkań musi być większa od 0.");
                isDataCorrect = false;
            }

            potentialErrors = string.Join("\n", errors);
        }
        #endregion

        #region Helpers

        public override void Save()
        {
            Validate();
            if (isDataCorrect)
            {
                    // Tworzenie nowego budynku
                    var newBuilding = new Buildings
                    {
                        ProjectID = SelectedProject.ProjectId,
                        BuildingName = BuildingName,
                        BuildingNumber = BuildingNumber,
                        Floors = Floors,
                        Status = "Zaplanowano"
                    };

                    estateEntities.Buildings.Add(newBuilding);
                    estateEntities.SaveChanges();

                    var newBuildingId = newBuilding.BuildingID;

                    // Tworzenie harmonogramu konstrukcji
                    var newConstructionSchedule = new ConstructionSchedule
                    {
                        ProjectID = SelectedProject.ProjectId,
                        Status = "Zaplanowano",
                        ConstructionPhase = "Zaplanowano",
                        StartDate = StartDate,
                        EndDate = EndDate,
                        BuildingId = newBuildingId,
                        
                    };

                    estateEntities.ConstructionSchedule.Add(newConstructionSchedule);
                    estateEntities.SaveChanges();

                    // Tworzenie mieszkań
                    for (int i = 1; i <= NumberOfApartments; i++)
                    {
                        var apartment = new Apartments
                        {
                            BuildingID = newBuildingId,
                            ApartmentNumber = i.ToString(),
                            Status = "Zaplanowane"
                        };

                        estateEntities.Apartments.Add(apartment);
                    }

                    estateEntities.SaveChanges();

                    Console.WriteLine("Zapis zakończony sukcesem.");
                
            }
            else
            {
                var errorModal = new ErrorModalView(potentialErrors);
                errorModal.ShowDialog();
            }
        }
        private void LoadProjects()
        {
            var projects = estateEntities.Projects.Select(c => new ProjectEntityForView
            {
                ProjectId = c.ProjectID,
                ProjectName = c.ProjectName,
                ProjectLocalization = c.Location
            }).ToList();

            foreach (var project in projects)
            {
                AvailableProject.Add(project);
            }
        }

        #endregion
    }
}
