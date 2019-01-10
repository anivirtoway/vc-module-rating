using System;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Rating.Core.Models;
using VirtoCommerce.Rating.Core.Services;
using VirtoCommerce.Rating.Data.Repositories;

namespace VirtoCommerce.Rating.Data.Services
{
    public class RatingSearchService : ServiceBase, IRatingSearchService
    {
        private readonly Func<IRatingRepository> _repositoryFactory;
        private readonly IRatingService _ratingService;

        public RatingSearchService(Func<IRatingRepository> repositoryFactory, IRatingService ratingService)
        {
            _repositoryFactory = repositoryFactory;
            _ratingService = ratingService;
        }

        public GenericSearchResult<CreateRatingDto> Search(RatingSearchCriteria criteria)
        {
            throw new NotImplementedException();

            if (criteria == null)
            {
                throw new ArgumentNullException($"{ nameof(criteria) } must be set");
            }

            var retVal = new GenericSearchResult<CreateRatingDto>();

            using (var repository = _repositoryFactory())
            {
                var query = repository.Ratings;

                if (!criteria.ProductId.IsNullOrEmpty())
                {
                    query = query.Where(x => criteria.ProductId == x.ProductId);
                }

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "CreatedDate", SortDirection = SortDirection.Descending } };
                }
                query = query.OrderBySortInfos(sortInfos);

                retVal.TotalCount = query.Count();

                var ratingIds = query.Skip(criteria.Skip)
                    .Take(criteria.Take)
                    .Select(x => x.Id)
                    .ToList();

                //retVal.Results = _ratingService.Get() GetByIds(ratingIds.ToArray())
                //    .OrderBy(x => ratingIds.IndexOf(x.Id)).ToList();
                return retVal;
            }


        }
    }
}
