namespace RestServer.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class addIndex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServerFormulas", "Name", c => c.String(maxLength: 20,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "CaseSensitive",
                        new AnnotationValues(oldValue: null, newValue: "True")
                    },
                }));
            CreateIndex("dbo.ServerFormulas", "Name", unique: true, name: "Ix_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ServerFormulas", "Ix_Name");
            AlterColumn("dbo.ServerFormulas", "Name", c => c.String(maxLength: 20,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "CaseSensitive",
                        new AnnotationValues(oldValue: "True", newValue: null)
                    },
                }));
        }
    }
}
