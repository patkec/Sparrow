using Sparrow.Domain.Models;

namespace Sparrow.Domain.Events
{
    public class OfferWonEvent
    {
        public Offer Offer { get; set; } 
    }
}