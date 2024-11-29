using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using System;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public class AddNewProjectViewModel : BaseDatabaseAdder<Projects>
    {
        #region Propeties
        public string ProjectName
        {
            get
            {
                return item.ProjectName;
            }
            set
            {
                item.ProjectName = value;
                OnPropertyChanged(() => ProjectName);
            }
        }

        public string Location
        {
            get
            {
                return item.Location;
            }
            set
            {
                item.Location = value;
                OnPropertyChanged(() => Location);
            }
        }
    
        public DateTime StartDate
        {
            get
            {
                return item?.StartDate?.Date ?? DateTime.MinValue;
            }
            set
            {
                item.StartDate = value;
                OnPropertyChanged(() => StartDate);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return item?.EndDate?.Date ?? DateTime.MinValue;
            }
            set
            {
                item.EndDate = value;
                OnPropertyChanged(() => EndDate);
            }
        }

        public string Status
        {
            get
            {
                return item.Status;
            }
            set
            {
                item.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        #endregion
        #region Constructor
        public AddNewProjectViewModel()
            : base()
        {
            item = new Projects();
        }


        #endregion

        #region Helpers

        public override void Save()
        {
            try
            {
                estateEntities.Projects.Add(item);
                estateEntities.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                    }
                }

                // Możesz rzucić wyjątek ponownie, aby debugować dalej
                throw;
            }
        }
        #endregion
    }
}