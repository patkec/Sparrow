﻿using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models.Drafts;

namespace Sparrow.Web.Controllers
{
    public class DraftsController: CrudApiController<OfferDraft, DraftViewModel, DraftDetailViewModel, DraftAddModel, DraftEditModel>
    {
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

        [HttpPost]
        [Route("api/drafts/{draftId}/items")]
        public HttpResponseMessage PostItem(Guid draftId, DraftItemAddModel model)
        {
            if (model == null || !model.ProductId.HasValue)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request.");

            var draft = Session.Get<OfferDraft>(draftId);
            if (draft == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Draft not found.");

            var product = Session.Load<Product>(model.ProductId.Value);
            var draftItem = new OfferDraftItem(product, model.Quantity)
            {
                Discount = model.Discount
            };

            draft.AddItem(draftItem);
            // Call Save explicitly to generate new Id which will be returned.
            Session.Save(draftItem);
            Session.Update(draft);

            var viewModel = Mapper.Map<DraftItemViewModel>(draftItem);
            return Request.CreateResponse(HttpStatusCode.Created, viewModel);
        }

        [HttpPut]
        [Route("api/drafts/{draftId}/items")]
        public HttpResponseMessage PutItem(Guid draftId, DraftItemEditModel model)
        {
            if (model == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request.");
            if (model.Id == Guid.Empty)
                return PostItem(draftId, model);
            if (model.ProductId.HasValue)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot update product for item.");

            var draftItem = Session.Get<OfferDraftItem>(model.Id);
            if (draftItem == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Draft item not found.");

            draftItem.Discount = model.Discount;
            draftItem.Quantity = model.Quantity;
            Session.Update(draftItem);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("api/drafts/{draftId}/items/{id}")]
        public HttpResponseMessage DeleteItem(Guid draftId, Guid id)
        {
            var draft = Session.Get<OfferDraft>(draftId);
            if (draft == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Draft not found");

            var draftItem = Session.Get<OfferDraftItem>(id);
            if (draftItem == null)
                return Request.CreateResponse(HttpStatusCode.OK);

            draft.RemoveItem(draftItem);
            Session.Update(draft);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/drafts/{draftId}/offer")]
        public HttpResponseMessage CreateOffer(Guid draftId/*, DateTime expiresOn*/)
        {
            //if (expiresOn < DateTime.Now)
            //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Offer expiry date should be in the future.");

            var draft = Session.Get<OfferDraft>(draftId);
            if (draft == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Draft not found.");

            var offer = draft.CreateOffer(DateTime.Now.AddDays(7));
            Session.Save(offer);

            return Request.CreateResponse(HttpStatusCode.OK, offer.Id);
        }
    }
}