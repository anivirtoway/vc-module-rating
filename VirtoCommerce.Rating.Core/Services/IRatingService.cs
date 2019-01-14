using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Rating.Core.Models;

namespace VirtoCommerce.Rating.Core.Services
{
    public interface IRatingService
    {
        float Calculate(int[] ratings, IRatingCalculator calculator);
        IList<IRatingCalculator> GetCalculators();
        Task<RatingDto> GetAsync(string storeId, string productId);
        Task<RatingDto[]> GetAsync(string storeId, string[] productIds);
        Task SaveAsync(CreateRatingDto[] createRatingsDto);
        void RecalculateAll(string storeId, IRatingCalculator calculator);
    }
}
