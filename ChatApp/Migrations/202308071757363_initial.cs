namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        user1 = c.String(nullable: false),
                        user1name = c.String(nullable: false),
                        user2name = c.String(nullable: false),
                        user2 = c.String(nullable: false),
                        timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        chatId = c.String(nullable: false),
                        senderId = c.String(nullable: false),
                        recieverId = c.String(nullable: false),
                        text = c.String(),
                        timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        name = c.String(nullable: false, maxLength: 30),
                        email_Id = c.String(nullable: false),
                        password = c.String(nullable: false, maxLength: 1024),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserModels");
            DropTable("dbo.MessageModels");
            DropTable("dbo.ChatModels");
        }
    }
}
