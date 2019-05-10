namespace Biblioteca_del_Papa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
                        BookName = c.String(),
                        Author = c.String(),
                        FinderID = c.Int(nullable: false),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Finders", t => t.FinderID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.FinderID);
            
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BookID = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        FinderID = c.Int(nullable: false),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Finders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Key = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CategoryAlias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
                        AliasName = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.CategoryAlias", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Books", "FinderID", "dbo.Finders");
            DropForeignKey("dbo.Chapters", "BookID", "dbo.Books");
            DropIndex("dbo.CategoryAlias", new[] { "CategoryID" });
            DropIndex("dbo.Chapters", new[] { "BookID" });
            DropIndex("dbo.Books", new[] { "FinderID" });
            DropIndex("dbo.Books", new[] { "CategoryID" });
            DropTable("dbo.CategoryAlias");
            DropTable("dbo.Categories");
            DropTable("dbo.Finders");
            DropTable("dbo.Chapters");
            DropTable("dbo.Books");
        }
    }
}
