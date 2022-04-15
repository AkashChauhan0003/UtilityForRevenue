using System;

namespace UtilityForRevenue.DomainModel
{
    public class InvoiceItemModel
    {
        public int ItemCategory { get; set; } //enum invoiceItemCategory
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public string IconUrl { get; set; }
        public int ItemType { get; set; }
        public string ItemTypeName { get; set; }
        public string Sku { get; set; }
        public decimal Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; } = 50;
        public decimal Tax { get; set; }
        public int Sequence { get; set; }
        public string ItemCategoryName { get; set; }
        public InvoicePersonInfo WhoIsThisFor { get; set; }
    }
    public class InvoicePersonInfo
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
    public class InvoicePersonInfo2
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
