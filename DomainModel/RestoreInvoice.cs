using System;

namespace UtilityForRevenue.DomainModel
{
    public class RestoreInvoice
    {
        public Guid InvoiceId { get; set; }
        public string TenantCode { get; set; }
    }
}
