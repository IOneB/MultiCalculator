namespace RestServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredFormulaString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServerFormulas", "FormulaString", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServerFormulas", "FormulaString", c => c.String());
        }
    }
}
