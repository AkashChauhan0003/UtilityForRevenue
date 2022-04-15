using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtilityForRevenue.DomainModel
{
    public class IssueCreditNote
    {

        public DateTime CreditNoteDate { get; set; }
        public Guid? CustomerId { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerType { get; set; }
        public decimal AmountToCredit { get; set; }
        public string Reason { get; set; }
        public int CreditNoteFor { get; set; }
        public List<CreditNoteInvoiceModel> Invoices { get; set; }
        public List<CreditNotePaymentModel> Payments { get; set; }
    }
    public class CreditNoteInvoiceModel
    {
        public decimal Amount { get; set; }
        public Guid InvoiceId { get; set; }
        public string Note { get; set; }
        public string InvoiceNumber { get; set; }
    }

    public class CreditNotePaymentModel
    {
        public string PaymentNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal RemainingPaymentAmount { get; set; }
        public decimal Amount { get; set; }
        public bool IsChecked { get; set; } = true;
        public string InvoiceNumber { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal CreditNoteIssuedAmount { get; set; }
        public Guid PaymentId { get; set; }
        public List<InvoiceSimpleModel> PaymentInvoices { get; set; }
    }
    public class InvoiceSimpleModel
    {
        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
