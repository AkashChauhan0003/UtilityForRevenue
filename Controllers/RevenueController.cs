using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UtilityForRevenue.DomainModel;
using UtilityForRevenue.RevenueServices;

namespace UtilityForRevenue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IRegistrationCancellationService _registrationCancel;
        private readonly ILogger<RevenueController> _logger;

        public RevenueController(IRegistrationCancellationService registrationCancel, ILogger<RevenueController> logger)
        {
            _registrationCancel = registrationCancel;
            _logger = logger;
        }

        [HttpPost("participantscancellation")]
        public async Task<IActionResult> CancelParticipants(CancelParticipants cancelParticipants)
        {
            try
            {
                if (cancelParticipants.CancellationPersonId == null || cancelParticipants.InvoiceId == null)
                    return Ok("Please Provide CancellledPerson and Invoice Id");
                var result = await _registrationCancel.RemoveRegistrantsFromInvoice(cancelParticipants);
                return Ok(result);

            }
            catch (Exception ex)
            {
                string message = string.Format("Utility failed for Invoice - {0} , please check inner exception here- {1}", cancelParticipants.InvoiceId,ex.Message);
                _logger.LogError(message);
                return Ok(ex.Message);
            }
        }
        [HttpPost("test")]
        public async Task<ReturnModel> test(CancelParticipants cancelParticipants)
        {
            var result = await _registrationCancel.RemoveRegistrantsFromInvoice(cancelParticipants);
            return result;
        }
        [HttpPost("GenrateCreditNote")]
        public async Task<IActionResult> GenrateCreditNote(CancelParticipants cancelParticipants)
        {
            try
            {
                if (cancelParticipants.CreditNoteAmount == 0 || cancelParticipants.InvoiceId == null)
                    return Ok("Please Provide CreditNoteAmount and Invoice Id");
                var result = await _registrationCancel.GenrateCreditNote(cancelParticipants);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string message = string.Format("Utility failed for Invoice - {0} , please check inner exception here- {1}", cancelParticipants.InvoiceId, ex.Message);
                _logger.LogError(message);
                return Ok(ex.Message);
            }
        }
        [HttpPost("GenrateAndUsedCreditNote")]
        public async Task<IActionResult> GenrateAndUsedCreditNote(GenrateCreditNote genrateCreditNote)
        {
            try
            {
                if (genrateCreditNote.AmountToCredit == 0 || genrateCreditNote.InvoiceId == null ||genrateCreditNote.CreditAppliedInvoiceId == null)
                    return Ok("Please Provide CreditNoteAmount and Invoice Id");
                var result = await _registrationCancel.GenrateAndUsedCreditNote(genrateCreditNote);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string message = string.Format("Utility failed for Invoice - {0} , please check inner exception here- {1}", genrateCreditNote.InvoiceId, ex.Message);
                _logger.LogError(message);
                return Ok(ex.Message);
            }
        }

        [HttpPost("RollbackInvoice")]
        public async Task<IActionResult> RollbackInvoice(Guid invoiceId,bool isForProd)
        {
            try
            {
                if (invoiceId == null)
                    return Ok("Please Provide Invoice Id");
                var result = await _registrationCancel.RollbackInvoice(invoiceId, isForProd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string message = string.Format("Utility failed for Invoice - {0} , please check inner exception here- {1}", invoiceId, ex.Message);
                _logger.LogError(message);
                return Ok(ex.Message);
            }
        }
    }
}
