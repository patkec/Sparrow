using System;

namespace Sparrow.Domain.Models
{
    public class Customer: EntityBase
    {
        private string _name;
        private string _email;
        private int _rating;

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the customer e-mail address.
        /// </summary>
        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// Gets or sets the rating for the customer.
        /// </summary>
        public virtual int Rating
        {
            get { return _rating; }
            set
            {
                if ((value < 0) || (value > 10))
                    throw new ArgumentOutOfRangeException("value");
                _rating = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        protected Customer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class with the specified name.
        /// </summary>
        /// <param name="name">Customer name.</param>
        public Customer(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _name = name;
        }
 
    }
}