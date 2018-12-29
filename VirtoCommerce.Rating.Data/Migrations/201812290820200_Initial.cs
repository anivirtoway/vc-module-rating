namespace VirtoCommerce.Rating.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductId = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Value = c.Single(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.StoreId, t.ProductId }, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Ratings", new[] { "StoreId", "ProductId" });
            DropTable("dbo.Ratings");
        }
    }
}
