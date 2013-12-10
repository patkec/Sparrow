using System;
using Sparrow.Domain.Models;
using Sparrow.Web.Models.Customers;
using Sparrow.Web.Models.Users;

namespace Sparrow.Web.Models.Offers
{
    public class OfferViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public UserViewModel Owner { get; set; }
        public CustomerViewModel Customer { get; set; }
        public decimal Total { get; set; }
        public OfferStatus Status { get; set; }
        public DateTime OfferedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}