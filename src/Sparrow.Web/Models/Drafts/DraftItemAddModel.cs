using System;

namespace Sparrow.Web.Models.Drafts
{
    public class DraftItemAddModel
    {
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public Guid? ProductId { get; set; }
    }
}