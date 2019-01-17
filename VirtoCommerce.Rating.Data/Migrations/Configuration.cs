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
    }
}
