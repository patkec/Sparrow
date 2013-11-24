using System;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Sparrow.Domain.Models;
using Sparrow.Web.Hubs;
using Sparrow.Web.Models;

namespace Sparrow.Web.Infrastructure
{
    public abstract class CrudApiController<TEntity, TViewModel, TAddModel, TEditModel>: SessionApiController
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
        /// Gets the offers messaging hub.
        /// </summary>
        protected IHubContext OffersHub
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<OffersHub>();
            }
        }

        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        public HttpResponseMessage Get(Guid id)
        {
            var entity = Session.Get<TEntity>(id);

            if (entity == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var model = Mapper.Map<TViewModel>(entity);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
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
        [ValidateModel]
        public HttpResponseMessage Put(TEditModel model)
        {
            if (model.Id == Guid.Empty)
                return Post(model);

            var entity = Session.Load<TEntity>(model.Id);
            Mapper.Map(model, entity);
            Session.Update(entity);
            OnEntityUpdated(entity);

            return Request.CreateResponse(HttpStatusCode.OK);
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