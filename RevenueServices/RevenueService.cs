using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UtilityForRevenue.DomainModel;
using UtilityForRevenue.Helper;
using UtilityForRevenue.RevenueRepositories;

namespace UtilityForRevenue.RevenueServices
{
    public class RevenueService : IRevenueService
    {
        public IRevenueRepository _revenueRepo;
        private readonly IConfiguration _configuration;
        public RevenueService(IRevenueRepository revenueRepo, IConfiguration configuration)
        {
            _revenueRepo = revenueRepo;
            _configuration = configuration;
        }
        public async Task<InvoiceCategory> GetStatusAndParticipants(CancelParticipants cancelParticipants)
        {
            InvoiceCategory category = new InvoiceCategory();
            var dt = await _revenueRepo.GetStatusAndParticipants(cancelParticipants.InvoiceId,cancelParticipants.IsForProduction);
            category.ParticipantsCount = Convert.ToInt32(dt.Rows[0][0].ToString());
            category.InvoiceStatus = Convert.ToInt32(dt.Rows[0][1].ToString());
            return category;
        }
        public async Task<string> VoidInvoice(CancelParticipants cancelParticipants)
        {

            try
            {
                VoidInvoice voidInvoice = new VoidInvoice();
                voidInvoice.InvoiceId = cancelParticipants.InvoiceId;
                voidInvoice.Remark = "Voiding an Invoice for cancellation of registration";
                // pass invoice id and remark on https://engagifii-preview7-revenue.azurewebsites.net/api/1.0/invoice/mark/void
                string data = JsonConvert.SerializeObject(voidInvoice);
                var url = "mark/void";
                return WebRequestHandler.Client(data, url,cancelParticipants.IsForProduction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreateUnpaidInvoice(CancelParticipants cancelParticipants)
        {
            try
            {
                List<InvoiceDetails> invoiceDetails = new List<InvoiceDetails>();
                var dt = await _revenueRepo.GetInvoiceDetail(cancelParticipants.InvoiceId, cancelParticipants.IsForProduction);
                invoiceDetails = ConvertDataTableToModel.ConvertToList<InvoiceDetails>(dt);
                var invoiceItem = await _revenueRepo.GetInvoiceItemDetail(cancelParticipants.InvoiceId, cancelParticipants.CancellationPersonId, cancelParticipants.IsForProduction);
                invoiceDetails[0].Items = ConvertDataTableToModel.ConvertToList<InvoiceItemModel>(invoiceItem);
                var WhoIsThisFor = ConvertDataTableToModel.ConvertToList<InvoicePersonInfo2>(invoiceItem);
                invoiceDetails[0].Items[0].WhoIsThisFor = new InvoicePersonInfo();
                invoiceDetails[0].Items[0].WhoIsThisFor.Id = WhoIsThisFor[0].Id;
                invoiceDetails[0].Items[0].WhoIsThisFor.Name = WhoIsThisFor[0].Name;
                invoiceDetails[0].Items[0].WhoIsThisFor.IconUrl = WhoIsThisFor[0].ImageUrl;
                invoiceDetails[0].CustomerType = invoiceDetails[0].CustomerTypeId == 1 ? "People" : "Organization";
                invoiceDetails[0].SubTotal = invoiceDetails[0].Items.Count * 50;
                invoiceDetails[0].Total = invoiceDetails[0].SubTotal;
                invoiceDetails[0].InvoiceDate = invoiceDetails[0].InvoiceDate.Year == DateTime.Today.Year ? new DateTime(2022,1,31): new DateTime(2021,12,31);
                invoiceDetails[0].InvoiceTime = invoiceDetails[0].InvoiceDate;
                invoiceDetails[0].DueDate = invoiceDetails[0].InvoiceDate.AddDays(30);
                invoiceDetails[0].Note = "Cancellation Invoice genrated against the invoice " + invoiceDetails[0].InvoiceNumber;
                invoiceDetails[0].InvoiceNumber = 0;
                string data = JsonConvert.SerializeObject(invoiceDetails[0]);
                // pass InvoiceDetails records as a json on https://engagifii-preview7-revenue.azurewebsites.net/api/1.0/invoice/create/
                var url = "invoice/create/";
                var response =  WebRequestHandler.Client(data,url,cancelParticipants.IsForProduction);
                return (JsonConvert.DeserializeObject<string>(response));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> IssueCreditNote(CancelParticipants cancelParticipants)
        {
            try
            {
                List<IssueCreditNote> creditNoteDetail = new List<IssueCreditNote>();
                var dt = await _revenueRepo.GetInvoiceDetailForCreditNote(cancelParticipants, cancelParticipants.IsForProduction);
                creditNoteDetail = ConvertDataTableToModel.ConvertToList<IssueCreditNote>(dt);
                creditNoteDetail[0].Invoices = ConvertDataTableToModel.ConvertToList<CreditNoteInvoiceModel>(dt);
                var paymentDetail = await _revenueRepo.GetPaymentDetailForCreditNote(cancelParticipants, cancelParticipants.IsForProduction);
                creditNoteDetail[0].Payments = ConvertDataTableToModel.ConvertToList<CreditNotePaymentModel>(paymentDetail);
                creditNoteDetail[0].CreditNoteDate = creditNoteDetail[0].Payments[0].PaymentDate.Year == DateTime.Today.Year ? new DateTime(2022, 1, 31) : new DateTime(2021, 12, 31);
                creditNoteDetail[0].Payments[0].RemainingPaymentAmount = creditNoteDetail[0].Payments[0].Amount;
                creditNoteDetail[0].Payments[0].PaymentAmount = creditNoteDetail[0].Payments[0].Amount;
                creditNoteDetail[0].Reason = "CreditNote genrated for canceling participants";
                creditNoteDetail[0].CreditNoteFor = creditNoteDetail[0].CustomerTypeId;
                creditNoteDetail[0].CustomerType = creditNoteDetail[0].CustomerTypeId == 1 ? "People" : "Organization";
                creditNoteDetail[0].AmountToCredit = creditNoteDetail[0].AmountToCredit == 0 ? cancelParticipants.CreditNoteAmount : creditNoteDetail[0].AmountToCredit;
                string data = JsonConvert.SerializeObject(creditNoteDetail[0]);
                // pass creditNoteDetail records as a json on https://engagifii-preview7-revenue.azurewebsites.net/api/1.0/creditNote/issue
                var url = "creditNote/issue";
                var response = WebRequestHandler.Client(data, url,cancelParticipants.IsForProduction);
                return (JsonConvert.DeserializeObject<string>(response));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UseCreditNote(string creditNoteId, string invoiceId,int totalAppliedAmount,bool isForProd)
        {
            UseCreditNote useCredit = new UseCreditNote();
            useCredit.creditNoteId = creditNoteId;
            useCredit.InvoiceId = invoiceId;
            useCredit.CreditBalanceUsedAmount = totalAppliedAmount == 0 ? 50 : totalAppliedAmount;
            useCredit.Note = "Cancellation Fees used against the cancellation Invoice";
            string data = JsonConvert.SerializeObject(useCredit);
            // pass creditNoteDetail records as a json on https://engagifii-preview7-revenue.azurewebsites.net/api/1.0/creditNote/use
            var url = "creditNote/use";
            return WebRequestHandler.Client(data, url,isForProd);
        }
        public async Task<string> RemoveParticipantFromInvoice(CancelParticipants cancelParticipants)
        {
            RemoveRegistrant removeRegistrant = new RemoveRegistrant();
            List<Guid> personIds = new List<Guid>();
            removeRegistrant.InvoiceId = cancelParticipants.InvoiceId;
            personIds.Add(cancelParticipants.CancellationPersonId);
            removeRegistrant.PersonIds = personIds;
            string data = JsonConvert.SerializeObject(removeRegistrant);
            // pass removeRegistrant records as a json on https://engagifii-preview7-revenue.azurewebsites.net/api/1.0/Public/DeleteRegistrants
            var url = "Public/DeleteRegistrants";
            var response = WebRequestHandler.Client(data, url,cancelParticipants.IsForProduction);
            return response;
        }
        public async Task<InvoiceStatus> CheckInvoiceStatus(CancelParticipants cancelParticipants)
        {
            List<InvoiceStatus> invoiceStatus = new List<InvoiceStatus>();
            var dt = await _revenueRepo.CheckInvoiceStatus(cancelParticipants.InvoiceId, cancelParticipants.IsForProduction);
            invoiceStatus = ConvertDataTableToModel.ConvertToList<InvoiceStatus>(dt);
            return invoiceStatus[0];
        }
        public async Task<string> RestoreInvoiceToOriginalState(Guid invoiceId, bool isForProd)
        {
            RestoreInvoice restoreInvoice = new RestoreInvoice();
            restoreInvoice.InvoiceId = invoiceId;
            restoreInvoice.TenantCode = "VSBA";
            string data = JsonConvert.SerializeObject(restoreInvoice);
            // pass removeRegistrant records as a json on https://engagifii-preview7-revenue.azurewebsites.net/api/1.0/Public/RestoreInvoiceToOriginalState
            var url = "Public/RestoreInvoiceToOriginalState";
            var response =  WebRequestHandler.Client(data, url,isForProd);
            return (JsonConvert.DeserializeObject<string>(response));
        }
    }
}
