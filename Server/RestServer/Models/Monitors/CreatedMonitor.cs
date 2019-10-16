using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.Model;
using RestServer.Models.Tokens;
using RestServer.Services.Interface;

namespace RestServer.Models.Monitors
{
    /// <summary>
    /// Мониторит созданные объекты и ожидающие расчета
    /// </summary>
    class CreatedMonitor : MonitorStatusManager
    {
        public override async Task SearchAsync(IRepository<ServerFormula> repository, IFormulaCalculator formulaCalculator, IFormulaStatusManager statusManager)
        {
            this.repository = repository;
            this.formulaCalculator = formulaCalculator;
            this.statusManager = statusManager;

            var observerFormulas = await repository.GetAllAsync(f => f.Status == FormulaStatus.Waiting
                                                                || f.Status == FormulaStatus.ErrorRequirements
                                                                || f.Status == FormulaStatus.InProgress,
                                                                tracking: true,
                                                                withRequired: true);

            await ManageStatus(observerFormulas, FormulaStatus.InProgress); //Формулы теперь в самом процессе расчета
            await ProcessInProgress(observerFormulas);
            //Успешно завершаем формулы, которые не отпали с ошибками
            await ManageStatus(observerFormulas.Where(f => f.Status == FormulaStatus.InProgress), FormulaStatus.Success); 
        }

        private async Task ProcessInProgress(IEnumerable<ServerFormula> observerFormulas)
        {
            foreach (var formula in observerFormulas)
            {
                //Узнаем необходимые зависимости
                var formulaRequirements = formulaCalculator
                    .TokenDiscriminator
                    .ConstructTokens(formula.FormulaString)
                    ?.Where(token => token is FormulaToken)
                    ?.Select(token => token.Value);

                //Вытаскиваем те из них, что уже посчитаны
                var existsRequirements = await repository
                    .GetAllAsync(f => formulaRequirements.Contains(f.Name)
                                      && f.Status == FormulaStatus.Success, true);


                //Если количество необходимых и имеющихся формул в выражении не сошлось, то нужно ждать остальные
                if (existsRequirements.Count() != formulaRequirements.Count())
                {
                    statusManager
                        .ChangeStatus(formula, FormulaStatus.ErrorRequirements)
                        .Notify();
                    continue;
                }
                else
                    AllRequirementsReady(formula, existsRequirements);
            }
        }

        /// <summary>
        /// Вызывается когда все необходимые значения есть и остается только посчитать
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="requireExistsFormula"></param>
        private void AllRequirementsReady(ServerFormula formula, IEnumerable<ServerFormula> requireExistsFormula)
        {
            foreach (var require in requireExistsFormula) //Добавляем ссылки на необходимые значения в базу
                if (!formula.Required.Contains(require))
                    formula.Required.Add(require);
            try
            {
                formulaCalculator.Calculate(formula, requireExistsFormula.ToList()); // и Считаем
            }
            catch (DivideByZeroException)
            {
                statusManager.ChangeStatus(formula, FormulaStatus.DivideByZeroError);
            }
        }
    }
}

