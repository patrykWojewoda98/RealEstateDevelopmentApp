using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realEstateDevelopment.MVVM.Model.EntitiesForView
{
    public class PurchasesEntityForView
    {
        public int PurchaseID { get; set; }
        public string TypeOfPurchase { get; set; }
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
