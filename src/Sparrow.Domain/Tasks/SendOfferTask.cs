using System;
using System.Threading;
using Sparrow.Domain.Events;
using Sparrow.Domain.Models;
using Sparrow.Infrastructure.Events;
using Sparrow.Infrastructure.Tasks;

namespace Sparrow.Domain.Tasks
{
    public class SendOfferTask: BackgroundTask
    {
        public Guid DraftId { get; set; }

        public DateTime ExpiresOn { get; set; }

        protected override void Execute()
        {
            var draft = Session.Get<OfferDraft>(DraftId);
            if (draft == null)
                throw new InvalidOperationException("Draft not found.");

            var offer = draft.CreateOffer(ExpiresOn);

            SendEmailForOffer(offer);

            Session.Save(offer);
            Session.Update(draft);

            DomainEvents.Raise(new OfferSentEvent
            {
                OfferId = offer.Id,
                SentTime = DateTime.Now
            });
        }

        private void SendEmailForOffer(Offer offer)
        {
            // It can take some time to prepare and send an email.
            Thread.Sleep(500);
        }
    }
}