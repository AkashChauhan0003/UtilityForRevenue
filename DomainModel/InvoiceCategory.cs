using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtilityForRevenue.DomainModel
{
    public class InvoiceCategory
    {
        public int InvoiceStatus { get; set; }
        public int ParticipantsCount { get; set; }
    }
    public class InvoiceStatus
    {
        public int Status { get; set; }
        public Guid Id { get; set; }
        public Guid InitialTransactionId { get; set; }
    }
}
