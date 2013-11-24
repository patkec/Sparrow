using System;

namespace Sparrow.Domain.Models
{
    public class Product: EntityBase
    {
        private string _title;
        private string _description;
        private decimal _price;

        /// <summary>
        /// Gets or sets the product title.
        /// </summary>
        public virtual string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");
                _title = value;
            }
        }

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        public virtual decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _price = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        protected Product()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class with the specified title.
        /// </summary>
        /// <param name="title">Product title.</param>
        public Product(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");
            _title = title;
        }
    }
}