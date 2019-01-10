using System;
using VirtoCommerce.Rating.Data.Models;
using VirtoCommerce.Rating.Data.Repositories;

namespace VirtoCommerce.Rating.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<RatingRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RatingRepository context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var now = DateTime.UtcNow;
            var rnd = new Random();
            context.AddOrUpdate(new RatingEntity
            {
                Id = "1",
                ProductId = "8b7b07c165924a879392f4f51a6f7ce0",
                StoreId = "4974648a41df4e6ea67ef2ad76d7bbd4",
                CreatedDate = now,
                CreatedBy = "initial data seed",
                Value = (float)Math.Round(rnd.NextDouble() * 5, 2)
            });

            context.AddOrUpdate(new RatingEntity
            {
                Id = "2",
                ProductId = "f9330eb5ed78427abb4dc4089bc37d9f",
                StoreId = "4974648a41df4e6ea67ef2ad76d7bbd4",
                CreatedDate = now,
                CreatedBy = "initial data seed",
                Value = (float)Math.Round(rnd.NextDouble() * 5, 2)
            });

            context.AddOrUpdate(new RatingEntity
            {
                Id = "3",
                ProductId = "d154d30d76d548fb8505f5124d18c1f3",
                StoreId = "4974648a41df4e6ea67ef2ad76d7bbd4",
                CreatedDate = now,
                CreatedBy = "initial data seed",
                Value = (float)Math.Round(rnd.NextDouble() * 5, 2)
            });

        }
    }
}
