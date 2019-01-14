using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Rating.Data.Models;

namespace VirtoCommerce.Rating.Data.Repositories
{
    public class RatingRepository : EFRepositoryBase, IRatingRepository
    {
        public RatingRepository() { }

        public RatingRepository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public IQueryable<RatingEntity> Ratings => GetAsQueryable<RatingEntity>();

        public async Task<RatingEntity> GetAsync(string storeId, string productId)
        {
            return (await GetAsync(storeId, new[] { productId })).FirstOrDefault();
        }

        public async Task<RatingEntity[]> GetAsync(string storeId, string[] productIds)
        {
            return await Ratings.Where(x => x.StoreId == storeId && productIds.Contains(x.ProductId))
                                .ToArrayAsync();
        }

        public async Task<RatingEntity[]> GetAsync(string[] ids)
        {
            return ids.Length > 0
                ? await Ratings.Where(x => ids.Contains(x.Id)).ToArrayAsync()
                : new RatingEntity[0];
        }

        public async Task DeleteAsync(string storeId, string productId)
        {
            var rating = await GetAsync(storeId, productId);
            Remove(rating);
        }

        public void Delete(string[] ids)
        {
            var items = new List<RatingEntity>(ids.Length);
            var set = Set<RatingEntity>();
            foreach (var id in ids)
            {
                var item = new RatingEntity { Id = id };
                items.Add(item);
                set.Attach(item);
            }

            set.RemoveRange(items);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingEntity>()
                .ToTable("Ratings")
                .HasKey(x => x.Id)
                .Property(x => x.Id);

            modelBuilder.Entity<RatingEntity>()
                .HasIndex(x => new { x.StoreId, x.ProductId })
                .IsUnique();

            modelBuilder.Entity<RatingEntity>()
                .Property(x => x.ProductId)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<RatingEntity>()
                .Property(x => x.StoreId)
                .HasMaxLength(128)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
