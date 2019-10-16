using RestServer.Migrations;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace RestServer.App_Data
{
    class CustomApplicationDbConfiguration : DbConfiguration
    {
        public CustomApplicationDbConfiguration()
        {
            SetMigrationSqlGenerator(
                SqlProviderServices.ProviderInvariantName,
                () => new CustomSqlServerMigrationSqlGenerator());
        }
    }
}
