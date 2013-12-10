using System;
using Sparrow.Web.Models.Products;

namespace Sparrow.Web.Models.Offers
{
    public class OfferItemViewModel
    {
        public Guid Id { get; set; } 
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ItemSubtotal { get; set; }
        public decimal ItemTotal { get; set; }
    }
}