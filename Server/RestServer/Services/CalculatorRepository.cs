using RestServer.App_Data;
using RestServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RestServer.Services
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

        public FormulaRepository()
        {
            db = new CalculatorDbContext();
        }

        public async Task Create(ServerFormula entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
        }

        public async Task<IEnumerable<ServerFormula>> GetAll(Func<ServerFormula, bool> condition = null)
        {
            var formulas = await db.Formulas.AsNoTracking().ToListAsync();
            if (condition != null)
            {
                return formulas.Where(condition);
            }
            return formulas;
        }

        public async Task<ServerFormula> GetById(int id)
        {
            return await db.Formulas.FindAsync(id);
        }

        public async Task<ServerFormula> GetByName(string name)
        {
            return await db.Formulas.FirstOrDefaultAsync(f => f.Name == name);
        }

        public async Task Update(ServerFormula formula)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
