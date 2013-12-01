using System;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using Sparrow.Domain.Events;
using Sparrow.Infrastructure.Events;
using Sparrow.Web.Hubs;
using Sparrow.Web.Models.Offers;

namespace Sparrow.Web.App_Start
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