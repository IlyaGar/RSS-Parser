namespace RSSParser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RSSes",
                c => new
                    {
                        Headline = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Url = c.String(),
                        Source_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Headline)
                .ForeignKey("dbo.RssSources", t => t.Source_Id)
                .Index(t => t.Source_Id);
            
            CreateTable(
                "dbo.RssSources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RSSes", "Source_Id", "dbo.RssSources");
            DropIndex("dbo.RSSes", new[] { "Source_Id" });
            DropTable("dbo.RssSources");
            DropTable("dbo.RSSes");
        }
    }
}
