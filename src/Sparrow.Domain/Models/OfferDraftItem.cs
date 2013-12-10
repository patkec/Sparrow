using System;

namespace Sparrow.Domain.Models
{
    public class OfferDraftItem : EntityBase
    {
        private Product _product;
        private int _quantity;
        private int _discount;
        private OfferDraft _offerDraft;

        /// <summary>
        /// Gets the parent <see cref="OfferDraft"/>.
        /// </summary>
        public virtual OfferDraft OfferDraft
        {
            get { return _offerDraft; }
            protected internal set { _offerDraft = value; }
        }

        /// <summary>
        /// Gets the product on offer.
        /// </summary>
        public virtual Product Product
        {
            get { return _product; }
        }

        /// <summary>
        /// Gets or sets the quantity of products on offer.
        /// </summary>
        public virtual int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _quantity = value;
            }
        }

        /// <summary>
        /// Gets a discount for the current product.
        /// </summary>
        /// <remarks>
        /// Item discount can be applied only via <see cref="OfferDraft"/>.
        /// </remarks>
        public virtual int Discount
        {
            get { return _discount; }
            protected internal set
            {
                if ((value < 0) || (value > 100))
                    throw new ArgumentOutOfRangeException("value");
                _discount = value;
            }
        }

        /// <summary>
        /// Gets the discount amount for this item.
        /// </summary>
        public virtual decimal DiscountAmount
        {
            get
            {
                return ItemSubtotal*Discount/100m;
            }
        }

        /// <summary>
        /// Gets total amount that should be paid for this item.
        /// </summary>
        public virtual decimal ItemTotal
        {
            get
            {
                return ItemSubtotal - DiscountAmount;
            }
        }

        /// <summary>
        /// Gets the subtotal amount for the item.
        /// </summary>
        public virtual decimal ItemSubtotal
        {
            get
            {
                return (Quantity*Product.Price);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferDraftItem"/> class.
        /// </summary>
        protected OfferDraftItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferDraftItem"/> class.
        /// </summary>
        /// <param name="product"><see cref="Product"/> on offer.</param>
        /// <param name="quantity">Quantity of products on offer.</param>
        public OfferDraftItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            if (quantity < 1)
                throw new ArgumentOutOfRangeException("quantity", quantity, "Quantity should be positive.");

            _product = product;
            _quantity = quantity;
        }
    }
}