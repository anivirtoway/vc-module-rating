using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Web.Security;
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
        [ResponseType(typeof(float))]
        [CheckPermission(Permission = PredefinedPermissions.RatingRead)]
        public IHttpActionResult Get(string storeId, string productId)
        {
            var rating = _ratingService.Get(storeId, productId);
            return Ok(rating);
        }
    }
}
