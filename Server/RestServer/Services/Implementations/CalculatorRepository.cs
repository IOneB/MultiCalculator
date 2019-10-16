using RestServer.App_Data;
using RestServer.Models;
using RestServer.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RestServer.Services.Implementations
{
    /// <summary>
    /// Реализация репозитория формул
    /// </summary>
    public class FormulaRepository : IRepository<ServerFormula>, IDisposable
    {
        /// <summary>
        /// Контекст для работы с базой данных
        /// </summary>
        private readonly CalculatorDbContext db;
        private readonly IFormulaStatusManager statusManager;

        public IFormulaValidator Validator { get; }

        public FormulaRepository(CalculatorDbContext context, IFormulaValidator validator, IFormulaStatusManager statusManager)
        {
            db = context;
            Validator = validator;
            this.statusManager = statusManager;
        }

        public async Task<bool> CreateAsync(ServerFormula entity)
        {
            if (!Validator.Validate(entity))
                return false;
            db.Formulas.Add(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(ServerFormula entity)
        {
            //Сначала нужно убедиться, что формула не в состоянии пересчета
            var statusCorrect = await CheckStatus(entity, DateTime.UtcNow + TimeSpan.FromSeconds(25));
            if (!statusCorrect)
                return false;

            db.Formulas.Attach(entity);
            statusManager.ChangeStatus(entity, CommonLibrary.Model.FormulaStatus.Deleted);

            bool saveSucces = await db.SaveChangesAsync() > 0;

            statusManager.Notify();

            return saveSucces;
        }

        public async Task<IEnumerable<ServerFormula>> GetAllAsync(Expression<Func<ServerFormula, bool>> condition = null, bool tracking = false, bool withRequired = false, bool withEncapsulated = false)
        {
            IQueryable<ServerFormula> formulas = db.Formulas;
            if (!tracking)
                formulas = formulas.AsNoTracking();
            if (withRequired)
                formulas = formulas.Include(f => f.Required);
            if (withEncapsulated)
                formulas = formulas.Include(f => f.Encapsulated);
            if (condition != null)
                formulas = formulas.Where(condition);

            return await formulas.ToListAsync();
        }

        public async Task<ServerFormula> GetByIdAsync(int id)
        {
            return await db.Formulas.FindAsync(id);
        }

        public async Task<ServerFormula> GetByNameAsync(string name)
        {
            return await db.Formulas
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Name == name); ;
        }

        public async Task<bool> UpdateAsync(ServerFormula entity)
        {
            if (!Validator.Validate(entity))
                return false;

            var statusCorrect = await CheckStatus(entity, DateTime.UtcNow + TimeSpan.FromSeconds(25));
            if (!statusCorrect)
                return false;

            statusManager.ChangeStatus(entity, CommonLibrary.Model.FormulaStatus.Updated);
            db.Entry(entity).State = EntityState.Modified;

            bool saveSucces = await db.SaveChangesAsync() > 0;

            statusManager.Notify();

            return saveSucces;
        }

        /// <summary>
        /// Убедиться, что формула не в состоянии пересчет
        /// 25 секунд на ожидание
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        private async Task<bool> CheckStatus(ServerFormula entity, DateTime expirationTime)
        {
            while (entity.Status == CommonLibrary.Model.FormulaStatus.InProgress 
                    || expirationTime > DateTime.UtcNow)
            {
                await Task.Delay(100);
                if ((await GetByNameAsync(entity.Name)).Status != CommonLibrary.Model.FormulaStatus.InProgress)
                    return true;
            }
            return false;
        }

        public async Task DeleteAsync(int formulaId)
        {
            var entity = await db.Formulas.FindAsync(formulaId);
            db.Formulas.Remove(entity);
        }

        public async Task SaveAsync() => await db.SaveChangesAsync();

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
