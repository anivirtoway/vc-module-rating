using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Web.Security;
using VirtoCommerce.Rating.Core.Models;
using VirtoCommerce.Rating.Core.Services;
using VirtoCommerce.Rating.Web.Security;

namespace VirtoCommerce.Rating.Web.Controllers.Api
{
    [RoutePrefix("api/rating")]
    public class RatingController : ApiController
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(RatingStoreListDto))]
        [CheckPermission(Permission = PredefinedPermissions.RatingRead)]
        public async Task<IHttpActionResult> GetForStore([FromUri]string storeId, [FromUri]string[] productIds)
        {
            if (string.IsNullOrWhiteSpace(storeId) || productIds.Length == 0)
            {
                return Ok(new RatingStoreListDto());
            }
            var result = new RatingStoreListDto
            {
                StoreId = storeId,
                Ratings = await _ratingService.GetForStoreAsync(storeId, productIds)
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(RatingCatalogListDto))]
        [CheckPermission(Permission = PredefinedPermissions.RatingRead)]
        public async Task<IHttpActionResult> GetForCatalog([FromUri]string catalogId, [FromUri]string[] productIds)
        {
            if (string.IsNullOrWhiteSpace(catalogId) || productIds.Length == 0)
            {
                return Ok(new RatingCatalogListDto());
            }

            var result = new RatingCatalogListDto
            {
                CatalogId = catalogId,
                Ratings = await _ratingService.GetForCatalogAsync(catalogId, productIds)
            };
            return Ok(result);
        }

        [HttpPost]
        [Route("calculateProduct")]
        [ResponseType(typeof(RatingStoreListDto))]
        [CheckPermission(Permission = PredefinedPermissions.RatingUpdate)]
        public IHttpActionResult CalculateProduct([FromUri]string storeId, [FromUri]string[] productIds)
        {
            if (string.IsNullOrWhiteSpace(storeId) || productIds.Length == 0)
            {
                return Ok(new RatingStoreListDto());
            }

            var result = new RatingStoreListDto
            {
                StoreId = storeId,
                Ratings = _ratingService.Calculate(storeId, productIds)
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("calculateStore")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.RatingUpdate)]
        public async Task<IHttpActionResult> CalculateStore([FromUri]string storeId)
        {
            if (string.IsNullOrWhiteSpace(storeId))
            {
                return Ok(new RatingStoreListDto());
            }

            await _ratingService.ReCalculateForStoreAsync(storeId);

            return Ok();
        }
    }
}
