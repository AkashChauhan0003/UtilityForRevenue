using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtilityForRevenue.DomainModel
{
    public class VoidInvoice
    {
        public Guid InvoiceId { get; set; }
        public string Remark { get; set; }
    }
}
