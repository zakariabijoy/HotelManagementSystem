namespace HotelManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPictureEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccomodationPackagePictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccomodationPackageId = c.Int(nullable: false),
                        PictureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PictureId, cascadeDelete: true)
                .ForeignKey("dbo.AccomodationPackages", t => t.AccomodationPackageId, cascadeDelete: true)
                .Index(t => t.AccomodationPackageId)
                .Index(t => t.PictureId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccomodationPictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccomodationId = c.Int(nullable: false),
                        PictureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PictureId, cascadeDelete: true)
                .ForeignKey("dbo.Accomodations", t => t.AccomodationId, cascadeDelete: true)
                .Index(t => t.AccomodationId)
                .Index(t => t.PictureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccomodationPictures", "AccomodationId", "dbo.Accomodations");
            DropForeignKey("dbo.AccomodationPictures", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.AccomodationPackagePictures", "AccomodationPackageId", "dbo.AccomodationPackages");
            DropForeignKey("dbo.AccomodationPackagePictures", "PictureId", "dbo.Pictures");
            DropIndex("dbo.AccomodationPictures", new[] { "PictureId" });
            DropIndex("dbo.AccomodationPictures", new[] { "AccomodationId" });
            DropIndex("dbo.AccomodationPackagePictures", new[] { "PictureId" });
            DropIndex("dbo.AccomodationPackagePictures", new[] { "AccomodationPackageId" });
            DropTable("dbo.AccomodationPictures");
            DropTable("dbo.Pictures");
            DropTable("dbo.AccomodationPackagePictures");
        }
    }
}
