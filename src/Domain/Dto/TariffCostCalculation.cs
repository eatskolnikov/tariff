using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class TariffCostCalculation
    {
        public int ProductId { get; set; }
        public string TariffName { get; set; }
        public Decimal AnualCost { get; set; }
    }
}
