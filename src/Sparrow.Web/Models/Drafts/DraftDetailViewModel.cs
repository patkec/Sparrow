using System;
using System.Collections.Generic;

namespace Sparrow.Web.Models.Drafts
{
    public class DraftDetailViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OwnerFullName { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<DraftItemViewModel> Items { get; set; }
    }
}