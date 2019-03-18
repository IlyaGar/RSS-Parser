namespace RSSParser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedoneRss : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RSSes", "Source_Id", "dbo.RssSources");
            DropIndex("dbo.RSSes", new[] { "Source_Id" });
            AddColumn("dbo.RSSes", "SourceId", c => c.Int(nullable: false));
            DropColumn("dbo.RSSes", "Source_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RSSes", "Source_Id", c => c.Int());
            DropColumn("dbo.RSSes", "SourceId");
            CreateIndex("dbo.RSSes", "Source_Id");
            AddForeignKey("dbo.RSSes", "Source_Id", "dbo.RssSources", "Id");
        }
    }
}
