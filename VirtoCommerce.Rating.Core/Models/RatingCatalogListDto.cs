using System.Collections.Generic;

namespace VirtoCommerce.Rating.Core.Models
{
    public class RatingCatalogListDto
    {
        public string CatalogId { get; set; }
        public IList<RatingStoreDto> Ratings { get; set; }

        public RatingCatalogListDto()
        {
            Ratings = new List<RatingStoreDto>();
        }
    }
}
