﻿using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Rating.Core.Models
{
    public class RatingDto : AuditableEntity
    {
        public float Value { get; set; }
    }
}
