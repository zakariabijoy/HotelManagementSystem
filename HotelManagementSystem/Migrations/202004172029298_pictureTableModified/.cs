namespace HotelManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pictureTableModified : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccomodationPackagePictures", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.AccomodationPackagePictures", "AccomodationPackageId", "dbo.AccomodationPackages");
            DropForeignKey("dbo.AccomodationPictures", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.AccomodationPictures", "AccomodationId", "dbo.Accomodations");
            DropIndex("dbo.AccomodationPackagePictures", new[] { "AccomodationPackageId" });
            DropIndex("dbo.AccomodationPackagePictures", new[] { "PictureId" });
            DropIndex("dbo.AccomodationPictures", new[] { "AccomodationId" });
            DropIndex("dbo.AccomodationPictures", new[] { "PictureId" });
            AddColumn("dbo.Pictures", "AccomodationId", c => c.Int());
            AddColumn("dbo.Pictures", "AccomodationPackageId", c => c.Int());
            CreateIndex("dbo.Pictures", "AccomodationId");
            CreateIndex("dbo.Pictures", "AccomodationPackageId");
            AddForeignKey("dbo.Pictures", "AccomodationId", "dbo.Accomodations", "Id");
            AddForeignKey("dbo.Pictures", "AccomodationPackageId", "dbo.AccomodationPackages", "Id");
            DropTable("dbo.AccomodationPackagePictures");
            DropTable("dbo.AccomodationPictures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AccomodationPictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccomodationId = c.Int(nullable: false),
                        PictureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccomodationPackagePictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccomodationPackageId = c.Int(nullable: false),
                        PictureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Pictures", "AccomodationPackageId", "dbo.AccomodationPackages");
            DropForeignKey("dbo.Pictures", "AccomodationId", "dbo.Accomodations");
            DropIndex("dbo.Pictures", new[] { "AccomodationPackageId" });
            DropIndex("dbo.Pictures", new[] { "AccomodationId" });
            DropColumn("dbo.Pictures", "AccomodationPackageId");
            DropColumn("dbo.Pictures", "AccomodationId");
            CreateIndex("dbo.AccomodationPictures", "PictureId");
            CreateIndex("dbo.AccomodationPictures", "AccomodationId");
            CreateIndex("dbo.AccomodationPackagePictures", "PictureId");
            CreateIndex("dbo.AccomodationPackagePictures", "AccomodationPackageId");
            AddForeignKey("dbo.AccomodationPictures", "AccomodationId", "dbo.Accomodations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AccomodationPictures", "PictureId", "dbo.Pictures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AccomodationPackagePictures", "AccomodationPackageId", "dbo.AccomodationPackages", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AccomodationPackagePictures", "PictureId", "dbo.Pictures", "Id", cascadeDelete: true);
        }
    }
}
