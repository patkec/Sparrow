using System;
using Sparrow.Domain.Models;

namespace Sparrow.Domain.Events
{
    public class OfferSentEvent
    {
        public Offer Offer { get; set; } 
        public DateTime SentTime { get; set; }
    }
}