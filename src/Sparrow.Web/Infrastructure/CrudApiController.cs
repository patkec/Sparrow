using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Hubs;
using Sparrow.Web.Models;

namespace Sparrow.Web.Infrastructure
{
    public abstract class CrudApiController<TEntity, TViewModel, TDetailViewModel, TAddModel, TEditModel>: SessionApiController
        where TEntity: EntityBase
        where TEditModel: TAddModel, IEditModel
    {
        /// <summary>
        /// Gets the administrative messaging hub.
        /// </summary>
        protected IHubContext AdminHub
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<AdminHub>();
            }
        }

        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            var entity = Session.Get<TEntity>(id);

            if (entity == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var model = Mapper.Map<TDetailViewModel>(entity);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        /// <summary>
        /// Gets a paged list of customers.
        /// </summary>
        [HttpGet]
        public PagedListModel<TViewModel> Get([FromUri] PagedListRequestModel requestModel)
        {
            requestModel = requestModel ?? new PagedListRequestModel
            {
                PageSize = 20
            };
            var itemsToSkip = (requestModel.Page - 1) * requestModel.PageSize;

            var itemsQuery = Session.QueryOver<TEntity>();
            if (!string.IsNullOrEmpty(requestModel.Filter))
                itemsQuery.Where(CreateFilter(requestModel.Filter));
            if (!string.IsNullOrEmpty(requestModel.Sort))
                itemsQuery.UnderlyingCriteria.AddOrder(new Order(requestModel.Sort, requestModel.OrderAscending));
            itemsQuery
                .Skip(itemsToSkip)
                .Take(requestModel.PageSize);

            var items = itemsQuery.Future();
            var totalItems = itemsQuery.ToRowCountQuery().FutureValue<int>();

            return new PagedListModel<TViewModel>
            {
                Page = requestModel.Page,
                PageSize = requestModel.PageSize,
                TotalItems = totalItems.Value,
                TotalPages = (int)Math.Ceiling(totalItems.Value / (double)requestModel.PageSize),
                Items = Mapper.Map<IEnumerable<TViewModel>>(items)
            };
        }

        protected abstract Expression<Func<TEntity, bool>> CreateFilter(string filter);

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        [HttpPost]
        [ValidateModel]
        public HttpResponseMessage Post(TAddModel model)
        {
            var entity = CreateEntity(model);
            Session.Save(entity);
            OnEntityCreated(entity);

            var viewModel = Mapper.Map<TViewModel>(entity);
            return Request.CreateResponse(HttpStatusCode.Created, viewModel);
        }

        /// <summary>
        /// Creates a new entity based on the given model.
        /// </summary>
        protected virtual TEntity CreateEntity(TAddModel model)
        {
            return Mapper.Map<TEntity>(model);
        }

        /// <summary>
        /// Called when a new entity is created.
        /// </summary>
        /// <param name="entity">A newly created entity.</param>
        protected virtual void OnEntityCreated(TEntity entity)
        {
        }

        /// <summary>
        /// Creates or updates an entity.
        /// </summary>
        [HttpPut]
        [ValidateModel]
        public HttpResponseMessage Put(TEditModel model)
        {
            if (model.Id == Guid.Empty)
                return Post(model);

            var entity = Session.Load<TEntity>(model.Id);
            UpdateEntity(entity, model);
            Session.Update(entity);
            OnEntityUpdated(entity);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Updates the entity with values from given model.
        /// </summary>
        /// <param name="entity">Entity to be updated.</param>
        /// <param name="model">Model that holds new values.</param>
        protected virtual void UpdateEntity(TEntity entity, TEditModel model)
        {
            Mapper.Map(model, entity);
        }

        /// <summary>
        /// Called when an entity has been updated.
        /// </summary>
        /// <param name="entity">Entity that was updated.</param>
        protected virtual void OnEntityUpdated(TEntity entity)
        {
        }

        /// <summary>
        /// Deletes an entity with id.
        /// </summary>
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            var entityToDelete = Session.Get<TEntity>(id);
            if (entityToDelete != null)
            {
                Session.Delete(entityToDelete);
                OnEntityDeleted(entityToDelete);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Called when an entity has been deleted.
        /// </summary>
        /// <param name="entity">Entity that was deleted.</param>
        protected virtual void OnEntityDeleted(TEntity entity)
        {
        }
    }
}