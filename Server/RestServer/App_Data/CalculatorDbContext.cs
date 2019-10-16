using RestServer.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CommonLibrary.Model;

namespace RestServer.App_Data
{
    /// <summary>
    /// Контекст базы данных формул
    /// </summary>
    public class CalculatorDbContext : DbContext
    {
        public DbSet<ServerFormula> Formulas { get; set; }

        public CalculatorDbContext() : base("CalculatorConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Добавляется новая условность по чувствительности регистра
            modelBuilder.Conventions.Add(new AttributeToColumnAnnotationConvention<CaseSensitiveAttribute, bool>(
                "CaseSensitive",
                (property, attributes) => attributes.Single().IsEnabled));
            //Устанавливается индекс для имени формулы
            modelBuilder.Entity<ServerFormula>().HasIndex(f => f.Name).HasName("Ix_Name").IsUnique(true);
            // Связь формулы с самой собой в формате многие-ко-многим для удоств в обновлении связных данных
            modelBuilder.Entity<ServerFormula>()
                .HasMany(f => f.Required)
                .WithMany(f => f.Encapsulated)
                .Map(config =>
                    config.ToTable("FormulaJoin")
                        .MapLeftKey("ReqId")
                        .MapRightKey("EncId")
                 );
        }
    }
}
