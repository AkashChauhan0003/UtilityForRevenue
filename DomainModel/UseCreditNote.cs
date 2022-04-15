using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtilityForRevenue.DomainModel
{
    public class UseCreditNote
    {
        public string InvoiceId { get; set; }
        public string creditNoteId { get; set; }
        public int CreditBalanceUsedAmount { get; set; }
        public string Note { get; set; }
    }
}
