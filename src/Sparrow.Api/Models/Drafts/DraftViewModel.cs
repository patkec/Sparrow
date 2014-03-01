using System;

namespace Sparrow.Api.Models.Drafts
{
    public class DraftViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public string OwnerFullName { get; set; }
        public string CustomerName { get; set; }
    }
}