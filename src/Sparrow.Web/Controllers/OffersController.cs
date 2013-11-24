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
    public class OffersController : CrudApiController<Offer, OfferViewModel, OfferAddModel, OfferEditModel>
    {
        public OfferPagedListModel Get([FromUri] PagedListRequestModel requestModel)
        {
            requestModel = requestModel ?? new PagedListRequestModel
            {
                PageSize = 20
            };
            var productsToSkip = (requestModel.Page - 1) * requestModel.PageSize;

            var productsQuery = Session.QueryOver<Offer>()
                .Skip(productsToSkip)
                .Take(requestModel.PageSize);
            if (!string.IsNullOrEmpty(requestModel.Sort))
                productsQuery.UnderlyingCriteria.AddOrder(new Order(requestModel.Sort, requestModel.OrderAscending));

            var products = productsQuery.Future();
            var totalItems = productsQuery.ToRowCountQuery().FutureValue<int>();

            return new OfferPagedListModel
            {
                Page = requestModel.Page,
                PageSize = requestModel.PageSize,
                TotalItems = totalItems.Value,
                TotalPages = (int)Math.Ceiling(totalItems.Value / (double)requestModel.PageSize),
                Offers = Mapper.Map<IEnumerable<OfferViewModel>>(products)
            };
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
                .Where(x => x.Status == OfferStatus.New)
                .OrderBy(x => x.CreatedOn).Desc
                .Take(5)
                .List();

            return Mapper.Map<IEnumerable<OfferViewModel>>(offers);
        }

        [HttpGet]
        [Route("api/offers/soonToExpire")]
        public IEnumerable<OfferViewModel> GetSoonToExpire()
        {
            var offers = Session.QueryOver<Offer>()
                .Where(x => x.Status != OfferStatus.Won && x.Status != OfferStatus.Lost)
                .And(x => x.ExpiresOn.IsBetween(DateTime.Now).And(DateTime.Now.AddDays(2)))
                .List();

            return Mapper.Map<IEnumerable<OfferViewModel>>(offers);
        }

        [Route("api/offers/{offerId}")]
        public void PostItem(Guid offerId)
        {
            
        }

        protected override Offer CreateEntity(OfferAddModel model)
        {
            throw new NotImplementedException();
        }

        protected override void OnEntityCreated(Offer entity)
        {
            var viewModel = Mapper.Map<OfferViewModel>(entity);
            OffersHub.Clients.All.offerCreated(viewModel);
        }

        protected override void OnEntityUpdated(Offer entity)
        {
            var viewModel = Mapper.Map<OfferViewModel>(entity);
            OffersHub.Clients.All.offerUpdated(viewModel);
        }

        protected override void OnEntityDeleted(Offer entity)
        {
            var viewModel = Mapper.Map<OfferViewModel>(entity);
            OffersHub.Clients.All.offerDeleted(viewModel);
        }
    }
}
