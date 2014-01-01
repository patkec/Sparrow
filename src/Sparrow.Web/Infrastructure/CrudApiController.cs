﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        where TAddModel: class
        where TEditModel: class, IEditModel
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
        public virtual IHttpActionResult Get(Guid id)
        {
            var entity = Session.Get<TEntity>(id);

            if (entity == null)
                return NotFound();

            var model = Mapper.Map<TDetailViewModel>(entity);
            return Ok(model);
        }

        /// <summary>
        /// Gets a paged list of customers.
        /// </summary>
        [HttpGet]
        public virtual PagedListModel<TViewModel> Get([FromUri] PagedListRequestModel requestModel)
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
        public virtual IHttpActionResult Post(TAddModel model)
        {
            var entity = CreateEntity(model);
            Session.Save(entity);
            OnEntityCreated(entity);

            var viewModel = Mapper.Map<TViewModel>(entity);
            return CreatedAtRoute("DefaultApi", new
            {
                id = entity.Id,
                controller = Request.GetActionDescriptor().ControllerDescriptor.ControllerName,
            }, viewModel);
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
        public virtual IHttpActionResult Put(TEditModel model)
        {
            if (model.Id == Guid.Empty)
            {
                var addModel = ConvertToAddModel(model);
                return Post(addModel);
            }

            var entity = Session.Load<TEntity>(model.Id);
            UpdateEntity(entity, model);
            Session.Update(entity);
            OnEntityUpdated(entity);

            return CreateUpdateResponse(entity);
        }

        /// <summary>
        /// Creates a response after specified entity has been updated.
        /// </summary>
        /// <param name="entity">Updated entity.</param>
        protected virtual IHttpActionResult CreateUpdateResponse(TEntity entity)
        {
            return Ok();
        }

        /// <summary>
        /// Converts given EditModel to AddModel so that it can be used with Post.
        /// </summary>
        protected virtual TAddModel ConvertToAddModel(TEditModel editModel)
        {
            if (editModel == null)
                return null;

            var addModel = editModel as TAddModel;
            if (addModel != null)
                return addModel;

            return Mapper.Map<TAddModel>(editModel);
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
        public virtual IHttpActionResult Delete(Guid id)
        {
            var entityToDelete = Session.Get<TEntity>(id);
            if (entityToDelete != null)
            {
                DeleteEntity(entityToDelete);
            }
            return Ok();
        }

        /// <summary>
        /// Deletes multiple entities with given identifiers.
        /// </summary>
        [HttpDelete]
        public virtual IHttpActionResult DeleteMany([FromUri] IEnumerable<Guid> ids)
        {
            var idArray = (ids == null) ? new Guid[0] : ids.ToArray();
            if (idArray.Length > 0)
            {
                var entitiesToDelete = Session.QueryOver<TEntity>()
                    .Where(x => x.Id.IsIn(idArray))
                    .List();
                foreach (var entity in entitiesToDelete)
                    DeleteEntity(entity);
            }
            return Ok();
        }

        private void DeleteEntity(TEntity entity)
        {
            Session.Delete(entity);
            OnEntityDeleted(entity);
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