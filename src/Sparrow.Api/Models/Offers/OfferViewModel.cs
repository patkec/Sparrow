using System;
using Sparrow.Domain.Models;
using Sparrow.Api.Models.Customers;
using Sparrow.Api.Models.Users;

namespace Sparrow.Api.Models.Offers
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