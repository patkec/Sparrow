using System;
using Sparrow.Web.Models.Products;

namespace Sparrow.Web.Models.Offers
{
    public class OfferItemViewModel
    {
        public Guid Id { get; set; } 
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}