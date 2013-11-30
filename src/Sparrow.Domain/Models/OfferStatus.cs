namespace Sparrow.Domain.Models
{
    public enum OfferStatus
    {
        /// <summary>
        /// Offer was sent to the customer.
        /// </summary>
        Offered,
        /// <summary>
        /// Customer has accepted the offer.
        /// </summary>
        Won,
        /// <summary>
        /// Customer has rejected the offer or the offer expired.
        /// </summary>
        Lost
    }
}