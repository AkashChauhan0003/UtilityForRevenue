using System;

namespace UtilityForRevenue.DomainModel
{
    public class GenrateCreditNote
    {
        public int AmountToCredit { get; set; }
        public Guid InvoiceId { get; set; }
        public string CreditAppliedInvoiceId { get; set; }
        public int TotalAmountApplied { get; set; }
        public bool IsForProduction { get; set; }
    }
}
