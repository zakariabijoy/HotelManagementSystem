namespace HotelManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescriptionIsAddedToAccomodationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accomodations", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accomodations", "Description");
        }
    }
}
