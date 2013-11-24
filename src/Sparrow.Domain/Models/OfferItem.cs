using System;

namespace Sparrow.Domain.Models
{
    public class OfferItem: EntityBase
    {
        private Product _product;
        private int _quantity;
        private double _discount;

        /// <summary>
        /// Gets the product included in the offer.
        /// </summary>
        public virtual Product Product
        {
            get { return _product; }
        }

        /// <summary>
        /// Gets or sets a number of products included in the offer.
        /// </summary>
        public virtual int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");
                _quantity = value;
            }
        }

        /// <summary>
        /// Gets or sets a discount percentage for the products on offer.
        /// </summary>
        public virtual double Discount
        {
            get { return _discount; }
            set
            {
                if (value > 100.0)
                    throw new ArgumentOutOfRangeException("value");
                _discount = value;
            }
        }

        /// <summary>
        /// Gets the total price of current <see cref="OfferItem"/>.
        /// </summary>
        public virtual decimal TotalPrice
        {
            get
            {
                return Quantity * Product.Price * (decimal)(100.0 - Discount);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferItem"/> class.
        /// </summary>
        protected OfferItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferItem"/> class for the specified product.
        /// </summary>
        /// <param name="product">Product that should be included in offer.</param>
        /// <param name="quantity">Number of products included in offer.</param>
        public OfferItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            if (quantity < 1)
                throw new ArgumentOutOfRangeException("quantity");

            _product = product;
            _quantity = quantity;
        } 
    }
}