using System;

namespace Sparrow.Web.Models.Drafts
{
    public class DraftViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OwnerName { get; set; }
        public string CustomerName { get; set; }
    }
}