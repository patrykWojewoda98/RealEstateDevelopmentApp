using System;
using System.Windows.Controls;
using System.Xml.Linq;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class ProjectEntityForView
    {
        public string ProjectName { get; set; }
        public string ProjectLocalization { get; set; }
        public int ProjectId {  get; set; } 
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public override string ToString()
        {
            return $"Projekt: {ProjectName} w {ProjectLocalization}";
        }
    }
}
