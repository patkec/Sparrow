using System;
using System.Xml.Serialization;
using Sparrow.Domain.Events;
using Sparrow.Domain.Models;

namespace Sparrow.Domain.Services
{
    public interface IOfferService
    {
        //void SendOffer(OfferDraft draft, DateTime expiresOn);
        void RejectOffer(Offer offer);
        void AcceptOffer(Offer offer);
    }

    class OfferService : IOfferService
    {
        public OfferService(IOrderService )
        {
            
        }

        public void RejectOffer(Offer offer)
        {
            if (offer == null)
                throw new ArgumentNullException("offer");

            offer.CompleteOffer(success: false);
            DomainEvents.Raise(new OfferLostEvent
            {
                Offer = offer
            });
        }

        public void AcceptOffer(Offer offer)
        {
            if (offer == null)
                throw new ArgumentNullException("offer");

            offer.CompleteOffer(success: true);
            DomainEvents.Raise(new OfferWonEvent
            {
                Offer = offer
            });
        }
    }

    public interface IOrderService
    {
        void CreateOrder(Offer offer);
    }
}