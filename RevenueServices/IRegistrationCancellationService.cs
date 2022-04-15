using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityForRevenue.DomainModel;

namespace UtilityForRevenue.RevenueServices
{
    public interface IRegistrationCancellationService
    {
        Task<ReturnModel> RemoveSingleRegistrants(CancelParticipants cancelParticipants);
        Task<ReturnModel> RemoveSingleRegistrantsWithFees(CancelParticipants cancelParticipants);
        Task<ReturnModel> RemoveSingleRegistrantsHavingMultiRegistrants(CancelParticipants cancelParticipants);
        Task<ReturnModel> RemoveSingleRegistrantsHavingMultiRegistrantsWithFee(CancelParticipants cancelParticipants);
        Task<ReturnModel> RemoveRegistrantsFromInvoice(CancelParticipants cancelParticipants);

        Task<string> GenrateCreditNote(CancelParticipants cancelParticipants);
        Task<string> GenrateAndUsedCreditNote(GenrateCreditNote genrateCreditNote);
        Task<string> RollbackInvoice(Guid invoiceId, bool isForProd);
    }
}
