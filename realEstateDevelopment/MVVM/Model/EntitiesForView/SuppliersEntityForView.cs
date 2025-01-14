using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class SuppliersEntityForView
    {
        #region Properties
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return CompanyName;
        }
        #endregion
    }
}
