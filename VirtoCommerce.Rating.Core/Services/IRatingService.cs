using System.Threading.Tasks;
using VirtoCommerce.Rating.Core.Models;

namespace VirtoCommerce.Rating.Core.Services
{
    public interface IRatingService
    {
        Task ReCalculateForStoreAsync(string storeId);
        RatingProductDto[] Calculate(string storeId, string[] productIds);
        Task<RatingProductDto> GetAsync(string storeId, string productId);
        Task<RatingProductDto[]> GetForStoreAsync(string storeId, string[] productIds);
        Task<RatingStoreDto[]> GetForCatalogAsync(string catalogId, string[] productIds);
        Task CreateOrUpdateAsync(string storeId, CreateRatingDto[] createRatingsDto);
    }
}
