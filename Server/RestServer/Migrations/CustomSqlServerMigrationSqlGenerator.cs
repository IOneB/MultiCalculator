using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServer.Migrations
{
    class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            // Делаем себе возможность избавиться от регистронезависимости
            base.Generate(alterColumnOperation);
            AnnotationValues values;
            if (alterColumnOperation.Column.Annotations.TryGetValue("CaseSensitive", out values))
            {
                if (values.NewValue != null && values.NewValue.ToString() == "True")
                {
                    using (var writer = Writer())
                    {
                        var columnSQL = BuildColumnType(alterColumnOperation.Column);
                        writer.WriteLine(
                            "ALTER TABLE {0} ALTER COLUMN {1} {2} COLLATE SQL_Latin1_General_CP1_CS_AS {3}",
                            alterColumnOperation.Table,
                            alterColumnOperation.Column.Name,
                            columnSQL,
                            alterColumnOperation.Column.IsNullable.HasValue == false || alterColumnOperation.Column.IsNullable.Value == true ? " NULL" : "NOT NULL"
                            );
                        Statement(writer);
                    }
                }
            }
        }
    }
}
