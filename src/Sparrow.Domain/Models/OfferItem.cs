using System;

namespace Sparrow.Domain.Models
{
    public class OfferItem: EntityBase
    {
        private Product _product;
        private int _quantity;
        private int _discount;
        private decimal _productPrice;
        private decimal _discountAmount;
        private decimal _itemSubtotal;
        private decimal _itemTotal;
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
        public virtual int Discount
        {
            get { return _discount; }
        }

        /// <summary>
        /// Gets a discount amount for current item.
        /// </summary>
        public virtual decimal DiscountAmount
        {
            get { return _discountAmount; }
        }

        /// <summary>
        /// Gets the subtotal price for current item.
        /// </summary>
        public virtual decimal ItemSubtotal
        {
            get { return _itemSubtotal; }
        }

        /// <summary>
        /// Gets the total price for current item.
        /// </summary>
        public virtual decimal ItemTotal
        {
            get { return _itemTotal; }
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
        /// <param name="draftItem"><see cref="OfferDraftItem"/> from which the offer item is created.</param>
        public OfferItem(OfferDraftItem draftItem)
        {
            if (draftItem == null)
                throw new ArgumentNullException("draftItem");

            _product = draftItem.Product;
            _quantity = draftItem.Quantity;
            _discount = draftItem.Discount;
            _productPrice = _product.Price;
            _discountAmount = draftItem.DiscountAmount;
            _itemTotal = draftItem.ItemTotal;
            _itemSubtotal = draftItem.ItemSubtotal;
        } 
    }
}