using RestServer.Models;
using RestServer.Services.Interface;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace RestServer.Controllers
{
    /// <summary>
    /// API-контроллер, обрабатывающий запросы пользователей
    /// </summary>
    public class CalculatorController : ApiController
    {
        private readonly IRepository<ServerFormula> repository;
        private readonly IFormulaMonitor monitor;

        public CalculatorController(IRepository<ServerFormula> repository, IFormulaMonitor monitor)
        {
            this.repository = repository;
            this.monitor = monitor;
        }

        /// <summary>
        /// GET-action, возвращающий список известных формул в формате JSON
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var result = await repository.GetAllAsync();
            return Json(result);
        }

        /// <summary>
        /// GET-action, возвращающий расчитанное значение формулы по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> Get(string name)
        {
            var result = await repository.GetByNameAsync(name);
            if (result == null)
                return NotFound();
            return Json(result);
        }

        /// <summary>
        /// GET-action, возвращающий расчитанное значение формулы по id 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var result = await repository.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Json(result);
        }

        /// <summary>
        /// POST-action, позволяющий создать формулу из тела запроса
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Post([FromBody][Bind(Include = "Name, FormulaString")]ServerFormula formula)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existsformula = await repository.GetByNameAsync(formula.Name); 
            if (existsformula != null)
                return Content(System.Net.HttpStatusCode.Conflict, "Такая формула уже существует");

            var success = await repository.CreateAsync(formula);
            if (success)
            {
                monitor.Created();
                return Created("api/calculator", "Запрос на создание формулы успешно создан");
            }
            else return InternalServerError(new Exception("Не удалось сохранить объект"));
        }

        /// <summary>
        /// PUT-action, позволяющий изменить значение формулы по идентификатору
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut]
        public async Task<IHttpActionResult> Update([FromBody][Bind(Include = "Name, FormulaString")]ServerFormula formula)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existsformula = await repository.GetByNameAsync(formula.Name);
            if (existsformula == null)
                return NotFound();

            existsformula.FormulaString = formula.FormulaString;
            var success = await repository.UpdateAsync(existsformula);
            if (success)
            {
                monitor.Updated();
                return Content(System.Net.HttpStatusCode.OK, "Запрос на обновление успешно создан");
            }
            else return InternalServerError(new Exception("Не удалось обновить объект"));
        }

        /// <summary>
        /// DELETE-action, позволяющий удалить формулу по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete]
        public async Task<IHttpActionResult> Delete(string name)
        {
            var formula = await repository.GetByNameAsync(name);
            if (formula == null)
                return NotFound();

            var success = await repository.DeleteAsync(formula);
            if (success)
            {
                monitor.Deleted();
                return Content(System.Net.HttpStatusCode.OK, "Запрос на удаление успешно создан");
            }
            else return InternalServerError(new Exception("Не удалось создать запрос на удаление объекта"));
        }
    }
}
