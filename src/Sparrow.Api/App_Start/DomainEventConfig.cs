using AutoMapper;
using Microsoft.AspNet.SignalR;
using Sparrow.Domain.Events;
using Sparrow.Api.Hubs;
using Sparrow.Api.Models.Offers;

namespace Sparrow.Api
{
    public class DomainEventConfig
    {
        public static void RegisterEvents()
        {
            var offerHub = GlobalHost.ConnectionManager.GetHubContext<OffersHub>();

            DomainEvents.Register((OfferWonEvent eventArgs) =>
            {
                var viewModel = Mapper.Map<OfferViewModel>(eventArgs.Offer);
                offerHub.Clients.All.offerWon(viewModel);
            });
            
            DomainEvents.Register((OfferLostEvent eventArgs) =>
            {
                var viewModel = Mapper.Map<OfferViewModel>(eventArgs.Offer);
                offerHub.Clients.All.offerLost(viewModel);
            });
            
            DomainEvents.Register((OfferSentEvent eventArgs) =>
            {
                var viewModel = Mapper.Map<OfferViewModel>(eventArgs.Offer);
                offerHub.Clients.All.offerSent(viewModel);
            });
        } 
    }
}