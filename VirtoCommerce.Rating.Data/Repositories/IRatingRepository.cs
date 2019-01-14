using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Rating.Data.Models;

namespace VirtoCommerce.Rating.Data.Repositories
{
    public interface IRatingRepository : IRepository
    {
        IQueryable<RatingEntity> Ratings { get; }
        Task<RatingEntity> GetAsync(string storeId, string productId);
        Task<RatingEntity[]> GetAsync(string storeId, string[] productIds);
        Task<RatingEntity[]> GetAsync(string[] ids);
        Task DeleteAsync(string storeId, string productId);
        void Delete(string[] ids);
    }
}
