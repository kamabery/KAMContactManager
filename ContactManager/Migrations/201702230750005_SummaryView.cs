namespace ContactManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SummaryView : DbMigration
    {
        public override void Up()
        {
            Sql(@"Create View SummaryView as
                Select a.AddressType type, count(*) TotalContacts from Contact C
                Inner join Addresses a on c.Id = a.ContactId
                Group By a.AddressType");
        }
        
        public override void Down()
        {
            Sql("Drop View SummaryView");
        }
    }
}
