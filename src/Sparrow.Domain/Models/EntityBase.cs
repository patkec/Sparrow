using System;

namespace Sparrow.Domain.Models
{
    public class EntityBase
    {
        private Guid _id;

        /// <summary>
        /// Gets or sets the identification for current entity.
        /// </summary>
        public virtual Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="EntityBase"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current entity.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as EntityBase);
        }

        /// <summary>
        /// Determines whether the specified <see cref="EntityBase"/> is equal to the current <see cref="EntityBase"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current entity.</param>
        protected bool Equals(EntityBase obj)
        {
            return (obj != null) && Id.Equals(obj.Id);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="EntityBase"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        } 
    }
}