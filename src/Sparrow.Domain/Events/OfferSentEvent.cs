using System;

namespace Sparrow.Domain.Events
{
    public class OfferSentEvent
    {
        public Guid OfferId { get; set; } 
        public DateTime SentTime { get; set; }
    }
}