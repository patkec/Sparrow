using System;
using Sparrow.Domain.Events;
using Sparrow.Domain.Models;
using Sparrow.Infrastructure.Commands;

namespace Sparrow.Web.Commands
{
    public class CloseOfferAsLostCommand: Command
    {
        public Guid OfferId { get; set; }

        protected override void Execute()
        {
            var offer = Session.Get<Offer>(OfferId);
            if (offer == null)
                throw new InvalidOperationException("Cannot find offer.");

            offer.CompleteOffer(false);
            // Some other things to do when offer is lost
            // ...

            Session.Update(offer);

            DomainEvents.Raise(new OfferLostEvent
            {
                Offer = offer
            });
        }
    }
}