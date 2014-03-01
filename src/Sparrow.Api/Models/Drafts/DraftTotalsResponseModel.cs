namespace Sparrow.Api.Models.Drafts
{
    public class DraftTotalsResponseModel
    {
        public int Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}