using System;
using System.Collections.Generic;
using Sparrow.Api.Models.Customers;

namespace Sparrow.Api.Models.Drafts
{
    public class DraftDetailViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime CreatedOn { get; set; }
        public string OwnerFullName { get; set; }
        public CustomerViewModel Customer { get; set; }
        public IEnumerable<DraftItemViewModel> Items { get; set; }
    }
}