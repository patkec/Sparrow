using System.Collections.Generic;

namespace Sparrow.Web.Models.Offers
{
    public class OfferPagedListModel : PagedListModel
    {
        public IEnumerable<OfferViewModel> Offers { get; set; }
    }
}