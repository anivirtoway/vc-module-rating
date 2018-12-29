using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Rating.Data.Models;

namespace VirtoCommerce.Rating.Data.Repositories
{
    public interface IRatingRepository : IRepository
    {
        RatingEntity Get(string storeId, string productId);
        RatingEntity[] Get(string[] ids);
        void Delete(string storeId, string productId);
        void Delete(string[] ids);
    }
}
