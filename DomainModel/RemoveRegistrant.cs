using System;
using System.Collections.Generic;

namespace UtilityForRevenue.DomainModel
{
    public class RemoveRegistrant
    {
        public Guid InvoiceId { get; set; }
        public List<Guid> PersonIds { get; set; }
    }
}
