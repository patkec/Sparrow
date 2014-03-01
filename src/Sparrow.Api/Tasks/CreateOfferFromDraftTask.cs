using System;
using Sparrow.Domain.Models;
using Sparrow.Infrastructure.Tasks;

namespace Sparrow.Api.Tasks
{
    public class CreateOfferFromDraftTask: BackgroundTask
    {
        public Guid DraftId { get; set; }

        public DateTime ExpiresOn { get; set; }

        protected override void Execute()
        {
            var draft = Session.Get<OfferDraft>(DraftId);
            if (draft == null)
                throw new InvalidOperationException("Draft not found.");

            var offer = draft.CreateOffer(ExpiresOn);
            Session.Save(offer);
            // We don't need the draft anymore
            Session.Delete(draft);

            TaskExecutor.ExecuteLater(new SendOfferEmailTask
            {
                OfferId = offer.Id
            });
        }
    }
}