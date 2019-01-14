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
        [ResponseType(typeof(RatingListDto))]
        [CheckPermission(Permission = PredefinedPermissions.RatingRead)]
        public async Task<IHttpActionResult> Get([FromUri]string storeId, [FromUri]string[] productIds)
        {
            if (string.IsNullOrWhiteSpace(storeId) || productIds.Length == 0)
            {
                return Ok(new RatingListDto
                {
                    StoreId = storeId,
                    Ratings = new RatingDto[0]
                });
            }
            var result = new RatingListDto
            {
                StoreId = storeId,
                Ratings = await _ratingService.GetAsync(storeId, productIds)
            };
            return Ok(result);
        }
    }
}
