using System;
using Sparrow.Domain.Models;

namespace Sparrow.Web.Models.Offers
{
    public class OfferViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OwnerName { get; set; }
        public string CustomerName { get; set; }
        public OfferStatus Status { get; set; }
        public decimal OfferPrice { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}