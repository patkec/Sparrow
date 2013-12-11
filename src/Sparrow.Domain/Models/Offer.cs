using System;
using System.Collections.Generic;
using System.Linq;

namespace Sparrow.Domain.Models
{
    /// <summary>
    /// Represents an offer that was sent to the customer. An offer cannot be altered.
    /// </summary>
    public class Offer: EntityBase
    {
        private Customer _customer;
        private User _owner;
        private string _title;
        private int _discount;
        private decimal _discountAmount;
        private decimal _subtotal;
        private decimal _total;
        private OfferStatus _status;
        private DateTime _offeredOn;
        private DateTime _expiresOn;
        private DateTime? _completedOn;
        private IList<OfferItem> _items = new List<OfferItem>();

        /// <summary>
        /// Gets the owner (or creator) of current offer.
        /// </summary>
        public virtual User Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// Gets the <see cref="Customer"/> that should get the offer.
        /// </summary>
        public virtual Customer Customer
        {
            get { return _customer; }
        }

        /// <summary>
        /// Gets the offer title.
        /// </summary>
        public virtual string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets the date when the offer was sent to the customer.
        /// </summary>
        public virtual DateTime OfferedOn
        {
            get { return _offeredOn; }
        }

        /// <summary>
        /// Gets the date when the offer expires.
        /// </summary>
        public virtual DateTime ExpiresOn
        {
            get { return _expiresOn; }
        }

        /// <summary>
        /// Gets the date when the offer was completed.
        /// </summary>
        public virtual DateTime? CompletedOn
        {
            get { return _completedOn; }
        }

        /// <summary>
        /// Gets a list of items included in the offer.
        /// </summary>
        public virtual IEnumerable<OfferItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets a discount percentage for the whole offer.
        /// </summary>
        public virtual int Discount
        {
            get { return _discount; }
        }

        /// <summary>
        /// Gets the discount amount for the offer.
        /// </summary>
        public virtual decimal DiscountAmount
        {
            get { return _discountAmount; }
        }

        /// <summary>
        /// Gets the subtotal for the offer.
        /// </summary>
        public virtual decimal Subtotal
        {
            get { return _subtotal; }
        }

        /// <summary>
        /// Gets the total price of the offer.
        /// </summary>
        public virtual decimal Total
        {
            get { return _total; }
        }

        /// <summary>
        /// Gets the offer status.
        /// </summary>
        public virtual OfferStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Offer"/> class.
        /// </summary>
        protected Offer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Offer"/> class for specified customer.
        /// </summary>
        /// <param name="draft">Source draft for the offer.</param>
        /// <param name="expiresOn"><see cref="DateTime"/> after which the offer is no longer valid.</param>
        public Offer(OfferDraft draft, DateTime expiresOn)
        {
            if (draft == null)
                throw new ArgumentNullException("draft");
            if (draft.Owner == null)
                throw new ArgumentException("Owner for the offer is not specified.", "draft");
            if (draft.Customer == null)
                throw new ArgumentException("Customer for the offer is not specified.", "draft");
            if (expiresOn < DateTime.Now)
                throw new ArgumentOutOfRangeException("expiresOn", expiresOn, "Offer expiry date should be in the future.");

            _owner = draft.Owner;
            _customer = draft.Customer;
            _title = draft.Title;
            _discount = draft.Discount;
            _expiresOn = expiresOn;
            _offeredOn = DateTime.Now;
            _discountAmount = draft.DiscountAmount;
            _subtotal = draft.Subtotal;
            _total = draft.Total;
        }

        /// <summary>
        /// Adds a new item to the offer.
        /// </summary>
        /// <param name="item"><see cref="OfferItem"/> that should be added to the offer.</param>
        protected internal virtual void AddItem(OfferItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (item.Offer != null)
                throw new ArgumentException("Item is already contained in an offer.");

            item.Offer = this;
            _items.Add(item);
        }

        /// <summary>
        /// Completes the offer.
        /// </summary>
        /// <param name="success">Indicates whether the offer was successful or not.</param>
        public virtual void CompleteOffer(bool success)
        {
            if (_status != OfferStatus.Offered)
                throw new InvalidOperationException("Offer was already completed.");

            _completedOn = DateTime.Now;
            _status = success ? OfferStatus.Won : OfferStatus.Lost;
        } 
    }
}