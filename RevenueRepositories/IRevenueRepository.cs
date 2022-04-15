using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UtilityForRevenue.DomainModel;

namespace UtilityForRevenue.RevenueRepositories
{
    public interface IRevenueRepository
    {
        Task<DataTable> GetInvoiceDetail(Guid invoiceId, bool isForProduction);
        Task<DataTable> CheckInvoiceStatus(Guid invoiceId, bool isForProduction);
        Task<DataTable> GetInvoiceDetailForCreditNote(CancelParticipants cancelParticipants, bool isForProduction);
        Task<DataTable> GetPaymentDetailForCreditNote(CancelParticipants cancelParticipants, bool isForProduction);
        Task<DataTable> GetInvoiceItemDetail(Guid invoiceId, Guid cancellationPersonId, bool isForProduction);
        Task<DataTable> GetStatusAndParticipants(Guid invoiceId, bool isForProduction);
    }
}
