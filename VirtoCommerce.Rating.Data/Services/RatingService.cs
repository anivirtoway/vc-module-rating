using System;
using System.Collections.Generic;
using System.Linq;
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

        public RatingDto Get(string storeId, string productId)
        {
            using (var repository = _repositoryFactory())
            {
                var rating = repository.Get(storeId, productId);
                return new RatingDto { Value = rating == null ? 0 : rating.Value };
            }
        }

        public void Save(CreateRatingDto[] createRatingsDto)
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
                    var alreadyExistEntities = repository.Get(alreadyExistIds);
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
