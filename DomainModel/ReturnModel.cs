using System;

namespace UtilityForRevenue.DomainModel
{
    public class ReturnModel
    {
        public Guid InvoiceId { get; set; }
        public Guid CancellationPersonId { get; set; }
        public string CancelledInvoiceId { get; set; }
        public string CreditNoteId { get; set; }
    }
}
