using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Offers;

namespace Sparrow.Web.Controllers
{
    public class OffersController: SessionApiController
    {
        [Route("api/offers")]
        public void Get(PagedListRequestModel model)
        {
            
        }

        [Route("api/offers/completed")]
        public void GetCompleted(PagedListRequestModel model)
        {
            
        }

        public void Get()
        {
            
        }

        [HttpGet]
        [Route("api/offers/{offerId}/items")]
        public HttpResponseMessage GetItems(Guid offerId)
        {
            var offer = Session.QueryOver<Offer>()
                .Fetch(x => x.Items).Eager
                .Where(x => x.Id == offerId)
                .SingleOrDefault();
            if (offer == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<OfferItemViewModel>>(offer.Items));
        }

        [HttpGet]
        [Route("api/offers/latest")]
        public IEnumerable<OfferViewModel> GetLatest()
        {
            var offers = Session.QueryOver<Offer>()
                .Where(x => x.Status == OfferStatus.Offered)
                .OrderBy(x => x.OfferedOn).Desc
                .Take(5)
                .List();

            return Mapper.Map<IEnumerable<OfferViewModel>>(offers);
        }

        [HttpGet]
        [Route("api/offers/soonToExpire")]
        public IEnumerable<OfferViewModel> GetSoonToExpire()
        {
            var offers = Session.QueryOver<Offer>()
                .Where(x => x.Status == OfferStatus.Offered)
                .And(x => x.ExpiresOn.IsBetween(DateTime.Now).And(DateTime.Now.AddDays(2)))
                .List();

            return Mapper.Map<IEnumerable<OfferViewModel>>(offers);
        }

        public HttpResponseMessage Put(Guid id, bool success)
        {
            var offer = Session.Get<Offer>(id);
            if (offer == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Offer not found.");

            offer.CompleteOffer(success);
            Session.Update(offer);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
