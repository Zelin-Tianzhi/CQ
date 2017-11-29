namespace CQ.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bus_Articles", "F_ArticleContent", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.Bus_Products", "F_Rule", c => c.String(unicode: false, storeType: "text"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bus_Products", "F_Rule", c => c.String());
            AlterColumn("dbo.Bus_Articles", "F_ArticleContent", c => c.String());
        }
    }
}
