using System;
using System.Collections.Generic;
using Sparrow.Web.Models.Customers;

namespace Sparrow.Web.Models.Drafts
{
    public class DraftDetailViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Discount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string OwnerFullName { get; set; }
        public CustomerViewModel Customer { get; set; }
        public IEnumerable<DraftItemViewModel> Items { get; set; }
    }
}