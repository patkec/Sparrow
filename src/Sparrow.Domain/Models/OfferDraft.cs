using System;
using System.Collections.Generic;
using System.Linq;

namespace Sparrow.Domain.Models
{
    /// <summary>
    /// Represents a draft of an offer. Drafts can be edited at will, but the offer cannot.
    /// </summary>
    public class OfferDraft : EntityBase
    {
        private string _title;
        private User _owner;
        private Customer _customer;
        private Offer _sourceOffer;
        private int _discount;
        private DateTime _createdOn;
        private IList<OfferDraftItem> _items = new List<OfferDraftItem>();

        /// <summary>
        /// Gets or sets the offer title.
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
        /// Gets or sets the owner for the offer.
        /// </summary>
        public virtual User Owner
        {
            get { return _owner; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _owner = value;
            }
        }

        /// <summary>
        /// Gets or sets the customer for the offer.
        /// </summary>
        public virtual Customer Customer
        {
            get { return _customer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _customer = value;
            }
        }

        /// <summary>
        /// Gets or sets the date when draft was created.
        /// </summary>
        public virtual DateTime CreatedOn
        {
            get { return _createdOn; }
        }

        /// <summary>
        /// Gets or sets the discount for the offer.
        /// </summary>
        public virtual int Discount
        {
            get { return _discount; }
            set
            {
                if ((value < 0) || (value > 100))
                    throw new ArgumentOutOfRangeException("value");
                _discount = value;
            }
        }

        /// <summary>
        /// Gets the discount amount for the offer.
        /// </summary>
        /// <remarks>
        /// Discounts for items are not included here.
        /// </remarks>
        public virtual decimal DiscountAmount
        {
            get
            {
                return Subtotal * Discount / 100m;
            }
        }

        /// <summary>
        /// Gets the subtotal for the offer.
        /// </summary>
        /// <remarks>
        /// Item discounts are already included here.
        /// </remarks>
        public virtual decimal Subtotal
        {
            get
            {
                return _items.Sum(x => x.ItemTotal);
            }
        }

        /// <summary>
        /// Gets the total price for the offer.
        /// </summary>
        public virtual decimal Total
        {
            get
            {
                return Subtotal - DiscountAmount;
            }
        }

        /// <summary>
        /// Gets the source offer on which this draft is based.
        /// </summary>
        public virtual Offer SourceOffer
        {
            get { return _sourceOffer; }
        }

        /// <summary>
        /// Gets a list of all items for the draft.
        /// </summary>
        public virtual IEnumerable<OfferDraftItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferDraft"/> class.
        /// </summary>
        protected OfferDraft()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferDraft"/> class.
        /// </summary>
        /// <param name="owner"><see cref="User"/> that has created the draft.</param>
        /// <param name="customer"><see cref="Customer"/> for which this offer is being created.</param>
        /// <param name="title">Title for the offer.</param>
        public OfferDraft(User owner, Customer customer, string title)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (customer == null)
                throw new ArgumentNullException("customer");
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");
            
            _owner = owner;
            _title = title;
            _customer = customer;
            _createdOn = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferDraft"/> class.
        /// </summary>
        /// <param name="sourceOffer">Source offer from which to create a new draft</param>
        private OfferDraft(Offer sourceOffer)
        {
            if (sourceOffer == null)
                throw new ArgumentNullException("sourceOffer");

            _sourceOffer = sourceOffer;
            _owner = sourceOffer.Owner;
            _title = sourceOffer.Title;
            _customer = sourceOffer.Customer;
            _discount = sourceOffer.Discount;
            _createdOn = DateTime.Now;
        }

        /// <summary>
        /// Adds a new item to the offer.
        /// </summary>
        /// <param name="item">An item to add to the offer.</param>
        public virtual void AddItem(OfferDraftItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (item.OfferDraft != null)
                throw new ArgumentException("Item is already contained in a draft.");
            item.OfferDraft = this;
            _items.Add(item);
        }

        /// <summary>
        /// Removes the specified item from the offer.
        /// </summary>
        /// <param name="item">An item to remove from the offer.</param>
        public virtual void RemoveItem(OfferDraftItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (!Equals(item.OfferDraft, this))
                throw new ArgumentException("Item is not part of current draft.", "item");

            item.OfferDraft = null;
            _items.Remove(item);
        }

        /// <summary>
        /// Changes discount for given item.
        /// </summary>
        /// <param name="itemId">Item identifier for which to change discount.</param>
        /// <param name="discount">Discount for the item.</param>
        public virtual void ChangeItemDiscount(Guid itemId, int discount)
        {
            var item = Items.FirstOrDefault(x => x.Id == itemId);
            if (item == null)
                throw new ArgumentException("Item with specified id does not exist.", "itemId");
            
            ChangeItemDiscount(item, discount);
        }

        /// <summary>
        /// Changes discount for given item.
        /// </summary>
        /// <param name="item">Item for which to change discount.</param>
        /// <param name="discount">Discount for the item.</param>
        public virtual void ChangeItemDiscount(OfferDraftItem item, int discount)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (!Items.Contains(item))
                throw new ArgumentException("Item is not part of current draft.", "item");

            // Dummy business rule - at most 5 items can have an additional discount.
            if ((discount > 0) && (item.Discount == 0))
            {
                var itemsWithDiscount = Items.Count(x => x.Discount > 0);
                if (itemsWithDiscount >= 5)
                    throw new InvalidOperationException("At most 5 items can have a special discount.");
            }
            item.Discount = discount;
        }

        /// <summary>
        /// Creates a new <see cref="Offer"/> based on current draft state.
        /// </summary>
        /// <param name="expiresOn"><see cref="DateTime"/> until which the offer is valid.</param>
        /// <returns>A new <see cref="Offer"/> instance.</returns>
        public virtual Offer CreateOffer(DateTime expiresOn)
        {
            if (expiresOn < DateTime.Now)
                throw new ArgumentOutOfRangeException("expiresOn");
            if (Owner == null)
                throw new InvalidOperationException("Owner should be defined before creating an offer.");
            if (Customer == null)
                throw new InvalidOperationException("Customer should be defined before creating an offer.");

            var offer = new Offer(this, expiresOn);
            foreach (var draftItem in Items)
                offer.AddItem(new OfferItem(draftItem));

            return offer;
        }

        /// <summary>
        /// Creates a new draft from given offer.
        /// </summary>
        /// <returns>A new <see cref="OfferDraft"/> instance.</returns>
        public static OfferDraft CreateFromOffer(Offer sourceOffer)
        {
            if (sourceOffer == null)
                throw new ArgumentNullException("sourceOffer");

            var draft = new OfferDraft(sourceOffer);
            foreach (var item in sourceOffer.Items)
            {
                var draftItem = new OfferDraftItem(item.Product, item.Quantity);
                draft.AddItem(draftItem);
                draft.ChangeItemDiscount(draftItem, item.Discount);
            }

            return draft;
        }
    }
}