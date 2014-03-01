using System;
using System.ComponentModel.DataAnnotations;

namespace Sparrow.Api.Models.Drafts
{
    public class DraftEditModel :  IEditModel
    {
        public Guid Id { get; set; }
        public int Discount { get; set; }
        [Required]
        public string Title { get; set; }
        public Guid CustomerId { get; set; }
    }
}