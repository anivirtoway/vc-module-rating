﻿using System;
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
            throw new System.NotImplementedException();
        }

        public float Get(string storeId, string productId)
        {
            using (var repository = _repositoryFactory())
            {
                return repository.Get(storeId, productId).Value;
            }
        }

        public void Save(RatingDto[] ratingsDto)
        {
            if (ratingsDto == null) throw new ArgumentNullException(nameof(ratingsDto));

            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                using (var changeTracker = GetChangeTracker(repository))
                {
                    var alreadyExistIds = ratingsDto.Where(x => x.IsTransient())
                                     .Select(x => x.Id)
                                     .ToArray();
                    var alreadyExistEntities = repository.Get(alreadyExistIds);
                    foreach (var rating in ratingsDto)
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
            throw new System.NotImplementedException();
        }
    }
}
