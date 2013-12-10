using System;

namespace Sparrow.Domain.Models
{
    public class OfferItem: EntityBase
    {
        private Product _product;
        private int _quantity;
        private double _discount;
        private decimal _productPrice;
        private Offer _offer;

        /// <summary>
        /// Gets the parent <see cref="Offer"/>.
        /// </summary>
        public virtual Offer Offer
        {
            get { return _offer; }
            protected internal set { _offer = value; }
        }

        /// <summary>
        /// Gets the product included in the offer.
        /// </summary>
        public virtual Product Product
        {
            get { return _product; }
        }

        /// <summary>
        /// Gets the price of the product as it was at the time of the offer.
        /// </summary>
        public virtual decimal ProductPrice
        {
            get { return _productPrice; }
        }

        /// <summary>
        /// Gets a number of products included in the offer.
        /// </summary>
        public virtual int Quantity
        {
            get { return _quantity; }
        }

        /// <summary>
        /// Gets a discount percentage for the products on offer.
        /// </summary>
        public virtual double Discount
        {
            get { return _discount; }
        }

        /// <summary>
        /// Gets the total price of current <see cref="OfferItem"/>.
        /// </summary>
        public virtual decimal TotalPrice
        {
            get
            {
                return Quantity * ProductPrice * (decimal)(1 - Discount / 100.0);
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
        /// <param name="discount">Discount percentage for the product.</param>
        public OfferItem(Product product, int quantity, double discount)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            if (quantity < 1)
                throw new ArgumentOutOfRangeException("quantity");

            _product = product;
            _quantity = quantity;
            _discount = discount;
            _productPrice = _product.Price;
        } 
    }
}