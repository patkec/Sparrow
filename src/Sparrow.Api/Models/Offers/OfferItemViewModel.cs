using System;
using Sparrow.Api.Models.Products;

namespace Sparrow.Api.Models.Offers
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