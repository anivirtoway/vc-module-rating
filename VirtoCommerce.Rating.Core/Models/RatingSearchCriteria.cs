using VirtoCommerce.Domain.Commerce.Model.Search;

namespace VirtoCommerce.Rating.Core.Models
{
    public class RatingSearchCriteria : SearchCriteriaBase
    {
        public string ProductId { get; set; }
        public string StoreId { get; set; }
    }
}
