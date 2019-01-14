namespace VirtoCommerce.Rating.Core.Models
{
    public class RatingListDto
    {
        public string StoreId { get; set; }
        public RatingDto[] Ratings { get; set; }
    }
}
