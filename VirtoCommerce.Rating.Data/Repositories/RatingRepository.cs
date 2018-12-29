using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Rating.Data.Models;

namespace VirtoCommerce.Rating.Data.Repositories
{
    public class RatingRepository : EFRepositoryBase, IRatingRepository
    {
        public RatingRepository()
        {
        }

        public RatingRepository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public IQueryable<RatingEntity> Ratings => GetAsQueryable<RatingEntity>();

        public RatingEntity Get(string storeId, string productId)
        {
            return Ratings.FirstOrDefault(x => x.StoreId == storeId && x.ProductId == productId);
        }

        public RatingEntity[] Get(string[] ids)
        {
            return ids.Length > 0
                ? Ratings.Where(x => ids.Contains(x.Id)).ToArray()
                : new RatingEntity[0];
        }

        public void Delete(string storeId, string productId)
        {
            var rating = Get(storeId, productId);
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
