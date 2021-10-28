using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        /*
         * if flat fee is true the calculation is:
         * 
         * if flat fee is false the calculation is:
         * base costs per month + (Price per kwh*Consumption)
         */
        public bool IsFlatFee { get; set; }

        //Pay as you go
        public decimal BaseCostPerMonth { get; set; }
        public decimal PricePerKwh { get; set; }


        //Flat fee fields
        public decimal FlatFeeBase { get; set; }
        public decimal FlatFeeLimitKwh { get; set; }
        public decimal FlatFeePricePerKwhAboveLimit { get; set; }

    }
}
