﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Infrastructure.Tasks;
using Sparrow.Api.Infrastructure;
using Sparrow.Api.Models.Drafts;
using Sparrow.Api.Security;
using Sparrow.Api.Tasks;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Sparrow.Api.Controllers
{
    public class DraftsController: CrudApiController<OfferDraft, DraftViewModel, DraftDetailViewModel, DraftAddModel, DraftEditModel>
    {
        private const string ResName = "Draft";
        protected override string ResourceName { get { return ResName; } }

        protected override Expression<Func<OfferDraft, bool>> CreateFilter(string filter)
        {
            return (draft => draft.Title.IsInsensitiveLike(filter, MatchMode.Anywhere));
        }

        protected override OfferDraft CreateEntity(DraftAddModel model)
        {
            var owner = Session.Load<User>(model.OwnerId);
            var customer = Session.Load<Customer>(model.CustomerId);

            return new OfferDraft(owner, customer, model.Title);
        }

        protected override void UpdateEntity(OfferDraft entity, DraftEditModel model)
        {
            entity.Title = model.Title;
            entity.Discount = model.Discount;
            entity.Customer = Session.Load<Customer>(model.CustomerId);
        }

        protected override IHttpActionResult CreateUpdateResponse(OfferDraft entity)
        {
            var response = Mapper.Map<DraftTotalsResponseModel>(entity);
            return Ok(response);
        }

        [HttpPost]
        [Route("api/drafts/create/{offerId}")]
        [ClaimsAuthorize(ResourceActionName.Create, ResName)]
        public IHttpActionResult CreateFromOffer(Guid offerId)
        {
            var offer = Session.Get<Offer>(offerId);
            if (offer == null)
                return NotFound();

            var draft = OfferDraft.CreateFromOffer(offer);
            Session.Save(draft);

            return CreatedAtRoute("DefaultApi", new {id = draft.Id, controller = "Drafts"}, draft.Id);
        }

        [HttpPost]
        [Route("api/drafts/{draftId}/items")]
        [ClaimsAuthorize(ResourceActionName.Update, ResName)]
        public IHttpActionResult PostItem(Guid draftId, DraftItemAddModel model)
        {
            if (model == null || !model.ProductId.HasValue)
                return BadRequest();

            var draft = Session.Get<OfferDraft>(draftId);
            if (draft == null)
                return NotFound();

            var product = Session.Load<Product>(model.ProductId.Value);
            var draftItem = new OfferDraftItem(product, model.Quantity);

            draft.AddItem(draftItem);
            draft.ChangeItemDiscount(draftItem, model.Discount);
            // Call Save explicitly to generate new Id which will be returned.
            Session.Save(draftItem);
            Session.Update(draft);

            var response = new DraftItemResponseModel
            {
                Draft = Mapper.Map<DraftTotalsResponseModel>(draft),
                Item = Mapper.Map<DraftItemViewModel>(draftItem)
            };
            return CreatedAtRoute("DefaultApi", new
            {
                id = draft.Id,
                controller = "Drafts"
            }, response);
        }

        [HttpPut]
        [Route("api/drafts/{draftId}/items")]
        [ClaimsAuthorize(ResourceActionName.Update, ResName)]
        public IHttpActionResult PutItem(Guid draftId, DraftItemEditModel model)
        {
            if (model == null)
                return BadRequest();
            if (model.Id == Guid.Empty)
                return PostItem(draftId, model);
            
            var draft = Session.Get<OfferDraft>(draftId);
            if (draft == null)
                return NotFound();

            var draftItem = draft.Items.FirstOrDefault(x => x.Id == model.Id);
            if (draftItem == null)
                return NotFound();

            draftItem.Quantity = model.Quantity;
            draft.ChangeItemDiscount(draftItem, model.Discount);
            Session.Update(draft);

            var response = new DraftItemResponseModel
            {
                Draft = Mapper.Map<DraftTotalsResponseModel>(draft),
                Item = Mapper.Map<DraftItemViewModel>(draftItem)
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("api/drafts/{draftId}/items/{id}")]
        [ClaimsAuthorize(ResourceActionName.Update, ResName)]
        public IHttpActionResult DeleteItem(Guid draftId, Guid id)
        {
            var draft = Session.Get<OfferDraft>(draftId);
            if (draft == null)
                return Ok();

            var draftItem = Session.Get<OfferDraftItem>(id);
            if (draftItem == null)
                return Ok();

            draft.RemoveItem(draftItem);
            Session.Update(draft);

            var response = Mapper.Map<DraftTotalsResponseModel>(draft);
            return Ok(response);
        }

        [HttpPost]
        [Route("api/drafts/{draftId}/offer")]
        [ClaimsAuthorize(ResourceActionName.Create, "Offer")]
        public HttpResponseMessage CreateOffer(Guid draftId, SendOfferModel model)
        {
            if ((model == null) || (model.ExpiresOn < DateTime.Now))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Offer expiry date should be in the future.");

            // Some parameter checking up-front
            var draft = Session.Get<OfferDraft>(draftId);
            if (draft == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Draft not found.");

            TaskExecutor.ExecuteLater(new CreateOfferFromDraftTask
            {
                DraftId = draftId,
                ExpiresOn = model.ExpiresOn
            });

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}