using System;
using System.Collections.Generic;

namespace UtilityForRevenue.DomainModel
{
    public class InvoiceDetails
    {
        public string Note { get; set; }
        public bool IsMarkVoidAllowed { get; set; } = true;
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceTime { get; set; }
        public Guid? RemittanceId { get; set; }
        public Guid[] RemittanceContacts { get; set; }
        public Guid? InvoiceRemittanceAddressId { get; set; }
        public long? InvoiceNumber { get; set; }
        public Guid PaymentTermId { get; set; }
        public string PaymentTerm { get; set; } = "Net 30";
        public DateTime? DueDate { get; set; }
        public string CustomerName { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerIcon { get; set; }
        public string CustomerType { get; set; }
        public int CustomerTypeId { get; set; }
        public Guid? InvoiceDeliveryAddressId { get; set; }
        public int SubTotal { get; set; }
        public int Total { get; set; }
        public int Status { get; set; } = 1;
        public DateTime? SendDate { get; set; }
        public List<InvoiceItemModel> Items { get; set; }

    }
}
