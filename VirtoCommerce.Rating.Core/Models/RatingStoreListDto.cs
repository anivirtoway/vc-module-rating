using System.Collections.Generic;

namespace VirtoCommerce.Rating.Core.Models
{
    public class RatingStoreListDto
    {
        public string StoreId { get; set; }
        public IList<RatingProductDto> Ratings { get; set; }

        public RatingStoreListDto()
        {
            Ratings = new List<RatingProductDto>();
        }
    }
}
