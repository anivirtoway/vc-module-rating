using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Rating.Data.Models
{
    public class RatingEntity : AuditableEntity
    {
        public string ProductId { get; set; }
        public string StoreId { get; set; }
        public float Value { get; set; }

        public virtual Core.Models.RatingDto ToModel(Core.Models.RatingDto toModel)
        {
            if (toModel == null)
                throw new ArgumentNullException(nameof(toModel));

            toModel.Id = Id;
            toModel.CreatedBy = CreatedBy;
            toModel.CreatedDate = CreatedDate;
            toModel.ModifiedBy = ModifiedBy;
            toModel.ModifiedDate = ModifiedDate;

            toModel.StoreId = StoreId;
            toModel.ProductId = ProductId;

            return toModel;
        }

        public virtual RatingEntity FromModel(Core.Models.RatingDto fromModel, PrimaryKeyResolvingMap pkMap)
        {
            if (fromModel == null)
                throw new ArgumentNullException(nameof(fromModel));

            pkMap.AddPair(fromModel, this);

            Id = fromModel.Id;
            CreatedBy = fromModel.CreatedBy;
            CreatedDate = fromModel.CreatedDate;
            ModifiedBy = fromModel.ModifiedBy;
            ModifiedDate = fromModel.ModifiedDate;

            StoreId = fromModel.StoreId;
            ProductId = fromModel.ProductId;

            return this;
        }

        public virtual void Patch(RatingEntity target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            target.StoreId = StoreId;
            target.ProductId = ProductId;
        }
    }
}
