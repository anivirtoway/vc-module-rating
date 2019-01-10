using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Rating.Core.Models
{
    public class CreateRatingDto : AuditableEntity
    {
        public string ProductId { get; set; }
        public string StoreId { get; set; }
        public float Value { get; set; }
    }
}
