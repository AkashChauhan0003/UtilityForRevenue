using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UtilityForRevenue.DomainModel;

namespace UtilityForRevenue.RevenueRepositories
{
    public class RevenueRepository : IRevenueRepository
    {
        private readonly IConfiguration _configuration;
        public RevenueRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DataTable> GetInvoiceDetail(Guid invoiceId, bool isForProduction)
        {
            DataTable Invoice = new DataTable();
            var connectionString = isForProduction ? _configuration.GetConnectionString("RevenueQA") : _configuration.GetConnectionString("RevenuePreview7");
            using (SqlConnection _con = new SqlConnection(connectionString))
            {
                string queryStatement = @"select i.Note, i.CreatedOn as 'InvoiceDate', i.CreatedOn as 'InvoiceTime',i.RemittanceId as 'RemittanceId', i.InvoiceRemittanceAddressId, i.PaymentTermId,i.InvoiceId as 'InvoiceNumber'
                                        ,i.DueDate, isnull(p.Id,i.OrganizationId) as 'CustomerId', i.InvoiceDeliveryAddressId, i.SendDate, i.InvoiceFor as 'CustomerTypeId' , ircp.Id as 'RemittanceContacts',  
                                        isnull(p.FirstName + concat( ' ',p.LastName),org.Name) as 'CustomerName',ISNULL(p.ImageUrl,org.ImageThumbUrl) as 'CustomerIcon'  from invoice i 
                                        left join InvoiceRemittanceContactPerson ircp on i.id = ircp.InvoiceId
                                        left join TenantUser tu on tu.Id = i.IndividualId
                                        left join Person p on p.id = tu.PersonId
                                        left join Organization org on org.id = i.OrganizationId
                                        where i.id = @InvoiceID";

                using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                {
                    _cmd.Parameters.Add(new SqlParameter("@InvoiceId", invoiceId));
                    Invoice = new DataTable("Invoices");
                    SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                    _con.Open();
                    _dap.Fill(Invoice);
                    _con.Close();
                }
            }
            return Invoice;
        }

        public async Task<DataTable> GetInvoiceItemDetail(Guid invoiceId, Guid cancellationPersonId, bool isForProduction)
        {
            DataTable Invoice = new DataTable();
            var connectionString = isForProduction ? _configuration.GetConnectionString("RevenueQA") : _configuration.GetConnectionString("RevenuePreview7");
            using (SqlConnection _con = new SqlConnection(connectionString))
            {
                string queryStatement = @"select i.ItemCategory,i.ItemId,i.ItemType, i.IconUrl,i.ItemName,i.ItemTypeName,i.Sku,i.Tax,i.Sequence,i.ItemCategoryName,
                                        p.Id, p.FirstName + concat( ' ',p.LastName) as 'Name',p.ImageUrl from InvoiceItem i 
                                        left join Person p on p.id = i.PersonId where InvoiceId = @InvoiceId and PersonId = @PersonId";

                using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                {
                    _cmd.Parameters.Add(new SqlParameter("@InvoiceId", invoiceId));
                    _cmd.Parameters.Add(new SqlParameter("@PersonId", cancellationPersonId));
                    Invoice = new DataTable("InvoiceItems");
                    SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                    _con.Open();
                    _dap.Fill(Invoice);
                    _con.Close();
                }
            }
            return Invoice;
        }
        public async Task<DataTable> GetInvoiceDetailForCreditNote(CancelParticipants cancelParticipants, bool isForProduction)
        {
            DataTable Invoice = new DataTable();
            var connectionString = isForProduction ? _configuration.GetConnectionString("RevenueQA") : _configuration.GetConnectionString("RevenuePreview7");
            using (SqlConnection _con = new SqlConnection(connectionString))
            {
                string queryStatement = "";
                if (cancelParticipants.CreditNoteAmount != 0)
                {
                    queryStatement = @"select i.Note, isnull(p.Id,i.OrganizationId) as 'CustomerId', i.InvoiceFor as 'CustomerTypeId', i.Id as 'InvoiceId', i.InvoiceNumber as 'InvoiceNumber'
                                        from invoice i 
                                        left join TenantUser tu on tu.Id = i.IndividualId
                                        left join Person p on p.id = tu.PersonId
                                        left join Organization org on org.id = i.OrganizationId
                                        where i.id = @InvoiceID";

                    using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                    {
                        _cmd.Parameters.Add(new SqlParameter("@InvoiceId", cancelParticipants.InvoiceId));
                        Invoice = new DataTable("Invoices");
                        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                        _con.Open();
                        _dap.Fill(Invoice);
                        _con.Close();
                    }
                }
                else 
                {
                    queryStatement = @"select i.Note, isnull(p.Id,i.OrganizationId) as 'CustomerId', i.InvoiceFor as 'CustomerTypeId', i.Id as 'InvoiceId', i.InvoiceNumber as 'InvoiceNumber',itm.AmountDecimal as 'AmountToCredit'
                                        from invoice i 
                                        left join TenantUser tu on tu.Id = i.IndividualId
                                        left join Person p on p.id = tu.PersonId
                                        left join Organization org on org.id = i.OrganizationId
                                        left join InvoiceItem itm on itm.InvoiceId = i.Id
                                        where i.id = @InvoiceID and itm.PersonId = @PersonId";

                    using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                    {
                        _cmd.Parameters.Add(new SqlParameter("@InvoiceId", cancelParticipants.InvoiceId));
                        _cmd.Parameters.Add(new SqlParameter("@PersonId", cancelParticipants.CancellationPersonId));
                        Invoice = new DataTable("Invoices");
                        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                        _con.Open();
                        _dap.Fill(Invoice);
                        _con.Close();
                    }

                }
                 
            }
            return Invoice;
        }

        public async Task<DataTable> GetPaymentDetailForCreditNote(CancelParticipants cancelParticipants, bool isForProduction)
        {
            DataTable Invoice = new DataTable(); 
            var connectionString = isForProduction ? _configuration.GetConnectionString("RevenueQA") : _configuration.GetConnectionString("RevenuePreview7");
            using (SqlConnection _con = new SqlConnection(connectionString))
            {
                string queryStatement = @"select Top(1) p.PaymentNumber, p.PaymentDate,p.Amount,p.Id as 'PaymentId'
                                        from Invoice i 
                                        left join InvoicePayment invP on invP.InvoiceId = i.Id
                                        left join Payment p on p.Id = invP.PaymentId
                                        where i.Id = @InvoiceID";

                using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                {
                    _cmd.Parameters.Add(new SqlParameter("@InvoiceId", cancelParticipants.InvoiceId));
                    Invoice = new DataTable("Payment");
                    SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                    _con.Open();
                    _dap.Fill(Invoice);
                    _con.Close();
                }
            }
            return Invoice;
        }
        public async Task<DataTable> CheckInvoiceStatus(Guid invoiceId, bool isForProduction)
        {
            DataTable Invoice = new DataTable();
            var connectionString = isForProduction ? _configuration.GetConnectionString("RevenueQA") : _configuration.GetConnectionString("RevenuePreview7");
            using (SqlConnection _con = new SqlConnection(connectionString))
            {
                string queryStatement = @"select Status,Id,InitialTransactionId from Invoice
                                        where Id = @InvoiceID";

                using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                {
                    _cmd.Parameters.Add(new SqlParameter("@InvoiceId", invoiceId));
                    Invoice = new DataTable("Status");
                    SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                    _con.Open();
                    _dap.Fill(Invoice);
                    _con.Close();
                }
            }
            return Invoice;
        }
        public async Task<DataTable> GetStatusAndParticipants(Guid invoiceId, bool isForProduction)
        {
            DataTable Invoice = new DataTable();
            var connectionString = isForProduction ? _configuration.GetConnectionString("RevenueQA") : _configuration.GetConnectionString("RevenuePreview7");
            using (SqlConnection _con = new SqlConnection(connectionString))
            {
                string queryStatement = @"select count(itm.Id), i.Status
                                        from invoice i 
                                        join InvoiceItem itm on itm.invoiceId = i.id
                                        where i.id = @InvoiceId
                                        GROUP BY i.status";

                using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                {
                    _cmd.Parameters.Add(new SqlParameter("@InvoiceId", invoiceId));
                    Invoice = new DataTable("Status");
                    SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                    _con.Open();
                    _dap.Fill(Invoice);
                    _con.Close();
                }
            }
            return Invoice;

        }
    }

}
