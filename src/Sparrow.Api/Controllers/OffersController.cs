using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Api.Commands;
using Sparrow.Api.Infrastructure;
using Sparrow.Api.Models;
using Sparrow.Api.Models.Offers;
using Sparrow.Api.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Sparrow.Api.Controllers
{
    public class OffersController: SessionApiController
    {
        private const string ResourceName = "Offer";

        [Route("api/offers")]
        [ClaimsAuthorize(ResourceActionName.List, ResourceName)]
        public PagedListModel<OfferViewModel> Get(PagedListRequestModel requestModel)
        {
            return GetOffers(requestModel, x => x.Status == OfferStatus.Offered);
        }

        [Route("api/offers/archived")]
        [ClaimsAuthorize(ResourceActionName.List, ResourceName)]
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
        [Route("api/offers/latest/{count}")]
        [ClaimsAuthorize(ResourceActionName.List, ResourceName)]
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
        [ClaimsAuthorize(ResourceActionName.List, ResourceName)]
        public IEnumerable<OfferViewModel> GetSoonToExpire(int? days)
        {
            var offers = Session.QueryOver<Offer>()
                .Where(x => x.Status == OfferStatus.Offered)
                .And(x => x.ExpiresOn.IsBetween(DateTime.Now).And(DateTime.Now.AddDays(days ?? 2)))
                .List();

            return Mapper.Map<IEnumerable<OfferViewModel>>(offers);
        }

        [HttpGet]
        [ClaimsAuthorize(ResourceActionName.Details, ResourceName)]
        public IHttpActionResult Get(Guid id)
        {
            var entity = Session.Get<Offer>(id);

            if (entity == null)
                return NotFound();

            var model = Mapper.Map<OfferDetailsViewModel>(entity);
            return Ok(model);
        }

        [HttpGet]
        [Route("api/offers/{offerId}/items")]
        [ClaimsAuthorize(ResourceActionName.Details, ResourceName)]
        public IHttpActionResult GetItems(Guid offerId)
        {
            var offer = Session.QueryOver<Offer>()
                .Fetch(x => x.Items).Eager
                .Where(x => x.Id == offerId)
                .SingleOrDefault();
            if (offer == null)
                return NotFound();

            return Ok(Mapper.Map<IEnumerable<OfferItemViewModel>>(offer.Items));
        }

        [HttpPut]
        [Route("api/offers/{offerId}/won")]
        [ClaimsAuthorize(ResourceActionName.Update, ResourceName)]
        public IHttpActionResult CloseAsWon(Guid id)
        {
            // Some parameter checking up-front
            var offer = Session.Get<Offer>(id);
            if (offer == null)
                return NotFound();

            var command = new CloseOfferAsWonCommand
            {
                OfferId = id
            };

            CommandExecutor.ExecuteCommand(command);
            return Ok();
        }

        [HttpPut]
        [Route("api/offers/{id}/archive")]
        [ClaimsAuthorize(ResourceActionName.Update, ResourceName)]
        public IHttpActionResult ArchiveOffer(Guid id)
        {
            // Some parameter checking up-front
            var offer = Session.Get<Offer>(id);
            if (offer == null)
                return NotFound();

            var command = new CloseOfferAsLostCommand
            {
                OfferId = id
            };

            CommandExecutor.ExecuteCommand(command);
            return Ok();
        }
    }
}
