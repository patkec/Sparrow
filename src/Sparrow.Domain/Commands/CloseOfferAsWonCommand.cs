using System;
using Sparrow.Domain.Events;
using Sparrow.Domain.Models;
using Sparrow.Infrastructure.Commands;
using Sparrow.Infrastructure.Events;

namespace Sparrow.Domain.Commands
{
    public class CloseOfferAsWonCommand: Command
    {
        public Guid OfferId { get; set; }

        protected override void Execute()
        {
            var offer = Session.Get<Offer>(OfferId);
            if (offer == null)
                throw new InvalidOperationException("Cannot find offer.");

            offer.CompleteOffer(true);
            // Some other things to do when offer is won, like creating an order
            // ...

            Session.Update(offer);

            DomainEvents.Raise(new OfferWonEvent
            {
                Offer = offer
            });
        }
    }
}