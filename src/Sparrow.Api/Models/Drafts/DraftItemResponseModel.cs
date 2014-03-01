namespace Sparrow.Api.Models.Drafts
{
    public class DraftItemResponseModel
    {
        public DraftTotalsResponseModel Draft { get; set; }
        public DraftItemViewModel Item { get; set; }
    }
}