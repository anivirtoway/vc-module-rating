using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Rating.Core.Models;
using VirtoCommerce.Rating.Core.Services;
using VirtoCommerce.Rating.Data.Models;
using VirtoCommerce.Rating.Data.Repositories;

namespace VirtoCommerce.Rating.Data.Services
{
    public class RatingService : ServiceBase, IRatingService
    {
        private readonly Func<IRatingRepository> _repositoryFactory;

        public RatingService(Func<IRatingRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public float Calculate(int[] ratings, IRatingCalculator calculator)
        {
            return calculator.Calculate(ratings);
        }

        public IList<IRatingCalculator> GetCalculators()
        {
            throw new NotImplementedException();
        }



        public async Task<RatingDto> GetAsync(string storeId, string productId)
        {
            return (await GetAsync(storeId, new[] { productId })).FirstOrDefault();
        }

        public async Task<RatingDto[]> GetAsync(string storeId, string[] productIds)
        {
            using (var repository = _repositoryFactory())
            {
                var ratings = await repository.GetAsync(storeId, productIds);
                if (ratings.Any())
                {
                    return ratings.Select(x => new RatingDto
                    {
                        Value = x.Value,
                        ProductId = x.ProductId
                    }).ToArray();
                }
            }
            return new RatingDto[0];
        }

        public async Task SaveAsync(CreateRatingDto[] createRatingsDto)
        {
            if (createRatingsDto == null) throw new ArgumentNullException(nameof(createRatingsDto));

            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                using (var changeTracker = GetChangeTracker(repository))
                {
                    var alreadyExistIds = createRatingsDto.Where(x => x.IsTransient())
                                     .Select(x => x.Id)
                                     .ToArray();
                    var alreadyExistEntities = await repository.GetAsync(alreadyExistIds);
                    foreach (var rating in createRatingsDto)
                    {
                        var source = AbstractTypeFactory<RatingEntity>.TryCreateInstance().FromModel(rating, pkMap);
                        var target = alreadyExistEntities.FirstOrDefault(x => x.Id == source.Id);
                        if (target != null)
                        {
                            changeTracker.Attach(target);
                            source.Patch(target);
                        }
                        else
                        {
                            repository.Add(source);
                        }
                    }

                    CommitChanges(repository);
                    pkMap.ResolvePrimaryKeys();
                }
            }
        }

        public void RecalculateAll(string storeId, IRatingCalculator calculator)
        {
            throw new NotImplementedException();
        }
    }
}
