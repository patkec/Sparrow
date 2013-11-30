using System;
using System.Collections.Generic;

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
        private double _discount;
        private IList<Offer> _offers = new List<Offer>();
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
        /// Gets or sets the discount for the offer.
        /// </summary>
        public virtual double Discount
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
        /// Gets a list of all offers created from current draft.
        /// </summary>
        public virtual IEnumerable<Offer> Offers
        {
            get { return _offers; }
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
        public OfferDraft(User owner, Customer customer, string title): this(owner, customer, title, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferDraft"/> class.
        /// </summary>
        /// <param name="owner"><see cref="User"/> that has created the draft.</param>
        /// <param name="customer"><see cref="Customer"/> for which this offer is being created.</param>
        /// <param name="title">Title for the offer.</param>
        /// <param name="sourceOffer">Source offer, if any.</param>
        public OfferDraft(User owner, Customer customer, string title, Offer sourceOffer)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (customer == null)
                throw new ArgumentNullException("customer");
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");
            _owner = owner;
            _title = title;
            _sourceOffer = sourceOffer;
        }

        /// <summary>
        /// Adds a new item to the offer.
        /// </summary>
        /// <param name="item">An item to add to the offer.</param>
        public virtual void AddItem(OfferDraftItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
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
            _items.Remove(item);
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
                offer.AddItem(new OfferItem(draftItem.Product, draftItem.Quantity, draftItem.Discount));
            _offers.Add(offer);

            return offer;
        }
    }
}