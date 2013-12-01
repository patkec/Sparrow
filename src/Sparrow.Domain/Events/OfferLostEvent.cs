using System;

namespace Sparrow.Domain.Events
{
    public class OfferLostEvent
    {
        public Guid OfferId { get; set; }
    }
}