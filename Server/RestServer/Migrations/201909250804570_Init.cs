namespace RestServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServerFormulas",
                c => new
                    {
                        FormulaId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                        Status = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                        FormulaString = c.String(),
                    })
                .PrimaryKey(t => t.FormulaId);
            
            CreateTable(
                "dbo.FormulaJoin",
                c => new
                    {
                        ReqId = c.Int(nullable: false),
                        EncId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReqId, t.EncId })
                .ForeignKey("dbo.ServerFormulas", t => t.ReqId)
                .ForeignKey("dbo.ServerFormulas", t => t.EncId)
                .Index(t => t.ReqId)
                .Index(t => t.EncId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FormulaJoin", "EncId", "dbo.ServerFormulas");
            DropForeignKey("dbo.FormulaJoin", "ReqId", "dbo.ServerFormulas");
            DropIndex("dbo.FormulaJoin", new[] { "EncId" });
            DropIndex("dbo.FormulaJoin", new[] { "ReqId" });
            DropTable("dbo.FormulaJoin");
            DropTable("dbo.ServerFormulas");
        }
    }
}
