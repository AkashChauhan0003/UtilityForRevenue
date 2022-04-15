using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityForRevenue.DomainModel;

namespace UtilityForRevenue.RevenueServices
{
    public class RegistrationCancellationService : IRegistrationCancellationService
    {
        private readonly IRevenueService _revenueService;
        public RegistrationCancellationService(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }
        public async Task<ReturnModel> RemoveRegistrantsFromInvoice(CancelParticipants cancelParticipants)
        {
            var invoiceStatus = await _revenueService.CheckInvoiceStatus(cancelParticipants);
            if (invoiceStatus.Status == 4)
            {
                // var Id = invoiceStatus.InitialTransactionId == null ? invoiceStatus.Id : invoiceStatus.InitialTransactionId;
                // cancelParticipants.InvoiceId = new Guid( await _revenueService.RestoreInvoiceToOriginalState(Id));
                cancelParticipants.InvoiceId = new Guid(await _revenueService.RestoreInvoiceToOriginalState(cancelParticipants.InvoiceId,cancelParticipants.IsForProduction));
            }
            var category = await _revenueService.GetStatusAndParticipants(cancelParticipants);
            if (category.InvoiceStatus == 2 && category.ParticipantsCount == 1)
                return await RemoveSingleRegistrants(cancelParticipants);
            else if (category.InvoiceStatus == 2 && category.ParticipantsCount > 1)
                return await RemoveSingleRegistrantsHavingMultiRegistrants(cancelParticipants);
            else if ((category.InvoiceStatus == 1 || category.InvoiceStatus == 3) && category.ParticipantsCount == 1)
                return await RemoveSingleRegistrantsWithFees(cancelParticipants);
            else if ((category.InvoiceStatus == 1 || category.InvoiceStatus == 3) && category.ParticipantsCount > 1)
                return await RemoveSingleRegistrantsHavingMultiRegistrantsWithFee(cancelParticipants);
            else
                return null;
        }
        public async Task<ReturnModel> RemoveSingleRegistrants(CancelParticipants cancelParticipants)
        {
            // remove single participant having unpaid invoice status and single invoiceItem
            ReturnModel returnModel = new ReturnModel();
            returnModel.InvoiceId = cancelParticipants.InvoiceId;
            returnModel.CancellationPersonId = cancelParticipants.CancellationPersonId;
            if (cancelParticipants.IsFeeApplicable)
                returnModel.CancelledInvoiceId = await _revenueService.CreateUnpaidInvoice(cancelParticipants);
            await _revenueService.VoidInvoice(cancelParticipants);
            return returnModel;
        }

        public async Task<ReturnModel> RemoveSingleRegistrantsWithFees(CancelParticipants cancelParticipants)
        {
            // remove single participant having paid invoice status and single invoiceItem
            ReturnModel returnModel = new ReturnModel();
            returnModel.InvoiceId = cancelParticipants.InvoiceId;
            returnModel.CancellationPersonId = cancelParticipants.CancellationPersonId;
            // Issue credit note against the cancellation person
            returnModel.CreditNoteId = await _revenueService.IssueCreditNote(cancelParticipants);
            if (cancelParticipants.IsFeeApplicable)
            { // create an cancellation invoice of 50$
                returnModel.CancelledInvoiceId = await _revenueService.CreateUnpaidInvoice(cancelParticipants);
                // use credit note for cancellation invoice of 50$
                await _revenueService.UseCreditNote(returnModel.CreditNoteId, returnModel.CancelledInvoiceId,0,cancelParticipants.IsForProduction);
            }
            // void the main invoice
            await _revenueService.VoidInvoice(cancelParticipants);
            return returnModel;
        }
        public async Task<ReturnModel> RemoveSingleRegistrantsHavingMultiRegistrants(CancelParticipants cancelParticipants)
        {
            // remove single participant having unpaid invoice status and multiple invoiceItem

            ReturnModel returnModel = new ReturnModel();
            returnModel.InvoiceId = cancelParticipants.InvoiceId;
            returnModel.CancellationPersonId = cancelParticipants.CancellationPersonId;
            if (cancelParticipants.IsFeeApplicable)
            {
                // create an cancellation invoice of 50$
                returnModel.CancelledInvoiceId = await _revenueService.CreateUnpaidInvoice(cancelParticipants);
            }
            // remove cancelled person from invoice
            await _revenueService.RemoveParticipantFromInvoice(cancelParticipants);
            return returnModel;
        }

        public async Task<ReturnModel> RemoveSingleRegistrantsHavingMultiRegistrantsWithFee(CancelParticipants cancelParticipants)
        {
            // remove single participant having paid invoice status and multiple invoiceItem

            ReturnModel returnModel = new ReturnModel();
            returnModel.InvoiceId = cancelParticipants.InvoiceId;
            returnModel.CancellationPersonId = cancelParticipants.CancellationPersonId;
            // Issue credit note against the cancellation person
            returnModel.CreditNoteId = await _revenueService.IssueCreditNote(cancelParticipants);
            if (cancelParticipants.IsFeeApplicable)
            { 
                // create an cancellation invoice of 50$
                returnModel.CancelledInvoiceId = await _revenueService.CreateUnpaidInvoice(cancelParticipants);
                // use credit note for cancellation invoice of 50$
                await _revenueService.UseCreditNote(returnModel.CreditNoteId, returnModel.CancelledInvoiceId,0,cancelParticipants.IsForProduction);
            }
            // remove cancelled person from invoice
            await _revenueService.RemoveParticipantFromInvoice(cancelParticipants);
            return returnModel;
        }

        public async Task<string> GenrateCreditNote(CancelParticipants cancelParticipants)
        {
            return await _revenueService.IssueCreditNote(cancelParticipants);
        }
        public async Task<string> GenrateAndUsedCreditNote(GenrateCreditNote genrateCreditNote)
        {
            CancelParticipants cancelParticipants = new CancelParticipants();
            cancelParticipants.CreditNoteAmount = genrateCreditNote.AmountToCredit;
            cancelParticipants.InvoiceId = genrateCreditNote.InvoiceId;
            cancelParticipants.IsForProduction = genrateCreditNote.IsForProduction;
            var creditNoteId = await _revenueService.IssueCreditNote(cancelParticipants);
            return await _revenueService.UseCreditNote(creditNoteId, genrateCreditNote.CreditAppliedInvoiceId,genrateCreditNote.TotalAmountApplied, genrateCreditNote.IsForProduction);
        }
        public async Task<string> RollbackInvoice(Guid invoiceId, bool isForProd)
        {
            return await _revenueService.RestoreInvoiceToOriginalState(invoiceId, isForProd);
        }
    }
   
}
