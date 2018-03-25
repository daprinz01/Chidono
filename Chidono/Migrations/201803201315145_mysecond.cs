namespace Chidono.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mysecond : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostedImagesTables",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ContentLenght = c.Int(nullable: false),
                        ContentType = c.String(),
                        FileName = c.String(),
                        InputeData = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostedImagesTableApplicationUsers",
                c => new
                    {
                        PostedImagesTable_Id = c.Guid(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PostedImagesTable_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.PostedImagesTables", t => t.PostedImagesTable_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.PostedImagesTable_Id)
                .Index(t => t.ApplicationUser_Id);
            
            DropColumn("dbo.AspNetUsers", "img");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "img", c => c.Binary());
            DropForeignKey("dbo.PostedImagesTableApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostedImagesTableApplicationUsers", "PostedImagesTable_Id", "dbo.PostedImagesTables");
            DropIndex("dbo.PostedImagesTableApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PostedImagesTableApplicationUsers", new[] { "PostedImagesTable_Id" });
            DropTable("dbo.PostedImagesTableApplicationUsers");
            DropTable("dbo.PostedImagesTables");
        }
    }
}
