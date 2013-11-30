using System;

namespace Sparrow.Web.Models.Drafts
{
    public class DraftEditModel : DraftAddModel, IEditModel
    {
        public Guid Id { get; set; }
    }
}