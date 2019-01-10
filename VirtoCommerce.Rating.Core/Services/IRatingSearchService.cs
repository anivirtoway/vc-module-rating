using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Rating.Core.Models;

namespace VirtoCommerce.Rating.Core.Services
{
    public interface IRatingSearchService
    {
        GenericSearchResult<CreateRatingDto> Search(RatingSearchCriteria criteria);
    }
}
