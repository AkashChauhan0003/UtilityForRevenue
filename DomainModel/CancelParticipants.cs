using System;

namespace UtilityForRevenue.DomainModel
{
    public class CancelParticipants
    {
        public Guid InvoiceId { get; set; }
        public bool IsFeeApplicable { get; set; }
        public bool IsForProduction { get; set; }
        public Guid CancellationPersonId { get; set; }
        public decimal CreditNoteAmount { get; set; }
     //   public string TenantCode { get; set; }

    }
}
