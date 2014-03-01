using System;
using System.Threading;
using Sparrow.Domain.Events;
using Sparrow.Domain.Models;
using Sparrow.Infrastructure.Tasks;

namespace Sparrow.Api.Tasks
{
    public class SendOfferEmailTask : BackgroundTask
    {
        public Guid OfferId { get; set; }

        protected override void Execute()
        {
            var offer = Session.Get<Offer>(OfferId);
            // It can take some time to prepare and send an email.
            Thread.Sleep(2000);
            // Now the offer is really sent, inform interested parties.
            DomainEvents.Raise(new OfferSentEvent
            {
                Offer = offer,
                SentTime = DateTime.Now
            });
        }
    }
}