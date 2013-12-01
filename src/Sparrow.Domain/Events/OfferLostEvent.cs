using System;
using Sparrow.Domain.Models;

namespace Sparrow.Domain.Events
{
    public class OfferLostEvent
    {
        public Offer Offer { get; set; }
    }
}