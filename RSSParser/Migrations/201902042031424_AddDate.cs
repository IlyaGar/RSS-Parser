namespace RSSParser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RSSes");
            AddPrimaryKey("dbo.RSSes", new[] { "Headline", "Date" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RSSes");
            AddPrimaryKey("dbo.RSSes", "Headline");
        }
    }
}
