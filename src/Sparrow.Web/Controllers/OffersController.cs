using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Commands;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Offers;

namespace Sparrow.Web.Controllers
{
    public class OffersController: SessionApiController
    {
        [Route("api/offers")]
        public PagedListModel<OfferViewModel> Get(PagedListRequestModel requestModel)
        {
            return GetOffers(requestModel, x => x.Status == OfferStatus.Offered);
        }

        [Route("api/offers/archived")]
        public PagedListModel<OfferViewModel> GetCompleted(PagedListRequestModel requestModel)
        {
            return GetOffers(requestModel, x => x.Status == OfferStatus.Won || x.Status == OfferStatus.Lost);
        }

        private PagedListModel<OfferViewModel> GetOffers(PagedListRequestModel requestModel, Expression<Func<Offer, bool>> filterExpression)
        {
            requestModel = requestModel ?? new PagedListRequestModel
            {
                PageSize = 20
            };
            var itemsToSkip = (requestModel.Page - 1) * requestModel.PageSize;

            var itemsQuery = Session.QueryOver<Offer>()
                .Where(filterExpression);
            if (!string.IsNullOrEmpty(requestModel.Filter))
                itemsQuery.Where(x => x.Title.IsInsensitiveLike(requestModel.Filter, MatchMode.Anywhere));
            if (!string.IsNullOrEmpty(requestModel.Sort))
                itemsQuery.UnderlyingCriteria.AddOrder(new Order(requestModel.Sort, requestModel.OrderAscending));
            itemsQuery
                .Skip(itemsToSkip)
                .Take(requestModel.PageSize);

            var items = itemsQuery.Future();
            var totalItems = itemsQuery.ToRowCountQuery().FutureValue<int>();

            return new PagedListModel<OfferViewModel>
            {
                Page = requestModel.Page,
                PageSize = requestModel.PageSize,
                TotalItems = totalItems.Value,
                TotalPages = (int)Math.Ceiling(totalItems.Value / (double)requestModel.PageSize),
                Items = Mapper.Map<IEnumerable<OfferViewModel>>(items)
            };
        }

        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            var entity = Session.Get<Offer>(id);

            if (entity == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var model = Mapper.Map<OfferDetailsViewModel>(entity);
            return Request.CreateResponse(HttpStatusCode.OK, model);
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
        [Route("api/offers/latest/{count}")]
        public IEnumerable<OfferViewModel> GetLatest(int? count)
        {
            var offers = Session.QueryOver<Offer>()
                .Where(x => x.Status == OfferStatus.Offered)
                .OrderBy(x => x.OfferedOn).Desc
                .Take(count ?? 5)
                .List();

            return Mapper.Map<IEnumerable<OfferViewModel>>(offers);
        }

        [HttpGet]
        [Route("api/offers/soonToExpire/{days}")]
        public IEnumerable<OfferViewModel> GetSoonToExpire(int? days)
        {
            var offers = Session.QueryOver<Offer>()
                .Where(x => x.Status == OfferStatus.Offered)
                .And(x => x.ExpiresOn.IsBetween(DateTime.Now).And(DateTime.Now.AddDays(days ?? 2)))
                .List();

            return Mapper.Map<IEnumerable<OfferViewModel>>(offers);
        }

        [HttpPut]
        [Route("api/offers/{offerId}/won")]
        public HttpResponseMessage CloseAsWon(Guid id)
        {
            // Some parameter checking up-front
            var offer = Session.Get<Offer>(id);
            if (offer == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Offer not found.");

            var command = new CloseOfferAsWonCommand
            {
                OfferId = id
            };

            CommandExecutor.ExecuteCommand(command);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPut]
        [Route("api/offers/{id}/archive")]
        public HttpResponseMessage ArchiveOffer(Guid id)
        {
            // Some parameter checking up-front
            var offer = Session.Get<Offer>(id);
            if (offer == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Offer not found.");

            var command = new CloseOfferAsLostCommand
            {
                OfferId = id
            };

            CommandExecutor.ExecuteCommand(command);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
