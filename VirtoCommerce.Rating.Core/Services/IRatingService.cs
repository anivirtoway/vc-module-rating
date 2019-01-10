using System.Collections.Generic;
using VirtoCommerce.Rating.Core.Models;

namespace VirtoCommerce.Rating.Core.Services
{
    public interface IRatingService
    {
        float Calculate(int[] ratings, IRatingCalculator calculator);
        IList<IRatingCalculator> GetCalculators();
        RatingDto Get(string storeId, string productId);
        void Save(CreateRatingDto[] createRatingsDto);
        void RecalculateAll(string storeId, IRatingCalculator calculator);
    }
}
