using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
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
        private readonly IUnityContainer _container;
        private readonly IStoreService _storeService;
        private readonly ICatalogService _catalogService;
        private readonly ICatalogSearchService _catalogSearchService;

        public RatingService(Func<IRatingRepository> repositoryFactory,
            IUnityContainer container,
            IStoreService storeService,
            ICatalogService catalogService,
            ICatalogSearchService catalogCatalogSearchService)
        {
            _repositoryFactory = repositoryFactory;
            _container = container;
            _storeService = storeService;
            _catalogService = catalogService;
            _catalogSearchService = catalogCatalogSearchService;
        }

        public RatingProductDto[] Calculate(string storeId, string[] productIds)
        {
            var calculator = GetCalculator(storeId, out var store);

            var allProductRatings = GetProductsReview(storeId, productIds);

            return allProductRatings.Select(productRatings => new RatingProductDto
            {
                ProductId = productRatings.Key,
                Value = calculator.Calculate(productRatings.Value)
            }).ToArray();
        }

        public async Task<RatingProductDto> GetAsync(string storeId, string productId)
        {
            return (await GetForStoreAsync(storeId, new[] { productId })).FirstOrDefault();
        }

        public async Task<RatingProductDto[]> GetForStoreAsync(string storeId, string[] productIds)
        {
            using (var repository = _repositoryFactory())
            {
                var ratings = await repository.GetAsync(storeId, productIds);
                if (ratings.Any())
                {
                    return ratings.Select(x => new RatingProductDto
                    {
                        Value = x.Value,
                        ProductId = x.ProductId
                    }).ToArray();
                }
            }
            return new RatingProductDto[0];
        }

        public async Task<RatingStoreDto[]> GetForCatalogAsync(string catalogId, string[] productIds)
        {
            var stores = GetStoresForCatalog(catalogId);
            var result = new List<RatingStoreDto>();

            using (var repository = _repositoryFactory())
            {
                foreach (var store in stores)
                {
                    var ratings = await repository.GetAsync(store.Id, productIds);
                    if (ratings.Any())
                    {
                        result.AddRange(ratings.Select(x => new RatingStoreDto
                        {
                            Value = x.Value,
                            StoreName = store.Name
                        }));
                    }
                }
            }
            return result.ToArray();
        }

        public async Task CreateOrUpdateAsync(string storeId, CreateRatingDto[] createRatingsDto)
        {
            if (createRatingsDto.Length == 0) return;

            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                using (var changeTracker = GetChangeTracker(repository))
                {
                    var productIds = createRatingsDto
                                     .Select(x => x.ProductId)
                                     .ToArray();
                    var alreadyExistEntities = await repository.GetAsync(storeId, productIds);
                    foreach (var rating in createRatingsDto)
                    {
                        var source = AbstractTypeFactory<RatingEntity>.TryCreateInstance().FromModel(rating, pkMap);
                        var target = alreadyExistEntities.FirstOrDefault(x =>
                            x.ProductId == source.ProductId
                            && x.StoreId == storeId);
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

        public async Task ReCalculateForStoreAsync(string storeId)
        {
            var calculator = GetCalculator(storeId, out var store);

            var productsCount = 0;
            var skip = 0;
            do
            {
                var searchResult = _catalogSearchService.Search(new Domain.Catalog.Model.SearchCriteria
                {
                    StoreId = storeId,
                    ResponseGroup = Domain.Catalog.Model.SearchResponseGroup.WithProducts,
                    Skip = skip
                });

                var productIds = searchResult.Products.Select(x => x.Id).ToList();

                var reviews = GetProductsReview(storeId, productIds);

                var productsWithReview = searchResult.Products.Where(x => reviews.Any(y => y.Key == x.Id));

                var ratingsToUpdate = productsWithReview.Select(x => new CreateRatingDto
                {
                    ProductId = x.Id,
                    StoreId = storeId,
                    Value = calculator.Calculate(reviews.FirstOrDefault(y => y.Key == x.Id).Value)
                }).ToArray();

                await CreateOrUpdateAsync(storeId, ratingsToUpdate);

                productsCount = searchResult.Products.Count;
                skip += productsCount;

            } while (productsCount > 0);
        }

        private IRatingCalculator GetCalculator(string storeId, out Store store)
        {
            store = _storeService.GetById(storeId);
            if (store == null)
            {
                throw new KeyNotFoundException($"Store not found, storeId: {storeId}");
            }

            var calculatorName = store.Settings.GetSettingValue<string>("Rating.Calculation.Method", null);
            if (string.IsNullOrWhiteSpace(calculatorName))
            {
                throw new KeyNotFoundException("Store settings not found: Rating.Calculation.Method");
            }

            if (!_container.IsRegistered<IRatingCalculator>(calculatorName))
            {
                throw new KeyNotFoundException($"{calculatorName} not found in DI container");
            }

            return _container.Resolve<IRatingCalculator>(calculatorName);
        }

        private Store[] GetStoresForCatalog(string catalogId)
        {
            var searchTotal = 0;
            var skip = 0;
            var result = new List<Store>();
            do
            {
                var search = _storeService.SearchStores(new SearchCriteria
                {
                    Skip = skip
                });
                var stores = search.Stores;
                searchTotal = search.TotalCount;
                skip += stores.Count;

                var catalogStores = stores.Where(x => x.Catalog == catalogId)
                                          .ToList();
                if (catalogStores.Any())
                {
                    result.AddRange(catalogStores);
                }
            }
            while (skip < searchTotal);

            return result.ToArray();
        }

        /// <summary>
        /// Test data
        /// todo: Rewrite to use Review table
        /// </summary>
        private Dictionary<string, int[]> GetProductsReview(string storeId, IList<string> productIds)
        {
            var result = new Dictionary<string, int[]>();
            if (storeId != "Electronics") { return result; }

            if (productIds.Contains("8b7b07c165924a879392f4f51a6f7ce0"))
            {
                result.Add("8b7b07c165924a879392f4f51a6f7ce0", new[] { 1, 1, 1, 4, 5, 1, 2, 3, 4, 5, 1, 1, 1 });
            }

            if (productIds.Contains("f9330eb5ed78427abb4dc4089bc37d9f"))
            {
                result.Add("f9330eb5ed78427abb4dc4089bc37d9f", new[] { 2, 2, 2, 4, 5, 1, 2, 3, 4, 5, 2, 2, 2 });
            }

            if (productIds.Contains("d154d30d76d548fb8505f5124d18c1f3"))
            {
                result.Add("d154d30d76d548fb8505f5124d18c1f3", new[] { 3, 3, 3, 4, 5, 1, 2, 3, 4, 5, 4, 4, 5 });
            }

            return result;
        }
    }
}
