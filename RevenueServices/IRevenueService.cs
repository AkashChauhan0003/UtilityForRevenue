using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UtilityForRevenue.DomainModel;

namespace UtilityForRevenue.RevenueServices
{
    public interface IRevenueService
    {
        Task<string> VoidInvoice(CancelParticipants cancelParticipants);
        Task<InvoiceStatus> CheckInvoiceStatus(CancelParticipants cancelParticipants);
        Task<InvoiceCategory> GetStatusAndParticipants(CancelParticipants cancelParticipants);
        Task<string> IssueCreditNote(CancelParticipants cancelParticipants);
        Task<string> CreateUnpaidInvoice(CancelParticipants cancelParticipants);
        Task<string> UseCreditNote(string creditNoteId, string invoiceId,int totalAppliedAmount,bool isForProd);
        Task<string> RemoveParticipantFromInvoice(CancelParticipants cancelParticipants);
        Task<string> RestoreInvoiceToOriginalState(Guid invoiceId,bool isForProd);
    }
}
