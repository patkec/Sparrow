using System;

namespace Sparrow.Web.Models.Offers
{
    public class OfferAddModel
    {
        public string Title { get; set; }
        public Guid OwnerId { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class OfferEditModel : OfferAddModel, IEditModel
    {
        public Guid Id { get; set; }
    }
}