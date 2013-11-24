using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sparrow.Domain.Models
{
    public class Offer: EntityBase
    {
        private Customer _customer;
        private User _owner;
        private string _title;
        private double _discount;
        private OfferStatus _status;
        private DateTime _createdOn;
        private DateTime _expiresOn;
        private IList<OfferItem> _items = new List<OfferItem>();

        /// <summary>
        /// Gets the owner (or creator) of current offer.
        /// </summary>
        public virtual User Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Customer"/> that should get the offer.
        /// </summary>
        public virtual Customer Customer
        {
            get { return _customer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                AssertNewOffer();
                _customer = value;
            }
        }

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
        /// Gets the date when the offer was created.
        /// </summary>
        public virtual DateTime CreatedOn
        {
            get { return _createdOn; }
        }

        /// <summary>
        /// Gets or sets the date when the offer expires.
        /// </summary>
        public virtual DateTime ExpiresOn
        {
            get { return _expiresOn; }
            set { _expiresOn = value; }
        }

        /// <summary>
        /// Gets a list of items included in the offer.
        /// </summary>
        public virtual IEnumerable<OfferItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets or sets a discount percentage for the whole offer.
        /// </summary>
        public virtual double Discount
        {
            get { return _discount; }
            set
            {
                if (value > 100.0)
                    throw new ArgumentOutOfRangeException("value");
                AssertNewOffer();
                _discount = value;
            }
        }

        /// <summary>
        /// Gets the total offer price.
        /// </summary>
        public virtual decimal OfferPrice
        {
            get
            {
                return _items.Sum(x => x.TotalPrice) * (decimal)(100.0 - Discount);
            }
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
        /// <param name="owner">Owner/creator of the offer.</param>
        /// <param name="customer">Customer which should get the offer.</param>
        /// <param name="title">Offer title.</param>
        public Offer(User owner, Customer customer, string title)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (customer == null)
                throw new ArgumentNullException("customer");
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            _owner = owner;
            _customer = customer;
            _title = title;
            _createdOn = DateTime.Now;
            _expiresOn = DateTime.Now.AddDays(7);
        }

        /// <summary>
        /// Adds a new item to the offer.
        /// </summary>
        /// <param name="item"><see cref="OfferItem"/> that should be added to the offer.</param>
        public virtual void AddItem(OfferItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            AssertNewOffer();
            _items.Add(item);
        }

        /// <summary>
        /// Removes specified item from the offer.
        /// </summary>
        /// <param name="item"><see cref="OfferItem"/> that should be removed from the offer.</param>
        public virtual void RemoveItem(OfferItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            AssertNewOffer();
            _items.Remove(item);
        }

        private void AssertNewOffer()
        {
            if (_status != OfferStatus.New)
                throw new InvalidOperationException("Offer cannot be modified after it was sent to the customer.");
        }

        /// <summary>
        /// Sends the offer to the customer.
        /// </summary>
        public virtual void SendOffer()
        {
            AssertNewOffer();
            _status = OfferStatus.Offered;
        }

        /// <summary>
        /// Completes the offer.
        /// </summary>
        /// <param name="success">Indicates whether the offer was successful or not.</param>
        public virtual void CompleteOffer(bool success)
        {
            if (_status == OfferStatus.New)
                throw new InvalidOperationException("Offer must be sent to the customer before it can be completed.");
            if (_status != OfferStatus.Offered)
                throw new InvalidOperationException("Offer was already completed.");

            _status = success ? OfferStatus.Won : OfferStatus.Lost;
        } 
    }
}