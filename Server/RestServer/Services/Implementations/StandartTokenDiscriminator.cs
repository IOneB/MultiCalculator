using RestServer.Models;
using RestServer.Models.Tokens;
using RestServer.Services.Interface;
using System;
using System.Linq;

namespace RestServer.Services.Implementations
{
    /// <summary>
    /// Реализация разделителя формулы на токены
    /// </summary>
    public class StandartTokenDiscriminator : ITokenDiscriminator
    {
        private readonly ILog logger;
        public TokenCollection Tokens { get; set; }

        public StandartTokenDiscriminator(ILog logger)
        {
            this.logger = logger;
        }

        public TokenCollection ConstructTokens(string exp)
        {
            if (string.IsNullOrEmpty(exp))
            {
                logger.Error("Значение формулы было неопределенным");
                return null;
            }

            var fplist = exp.Replace("(", " ( ").Replace(")", " ) ").Replace(".", ",");
            var stringTokens = Operator.GetAll
                .Aggregate(fplist, (list, op) => list.Replace(op.Name, $" {op.Name} "))
                .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Tokens = new TokenCollection();
            Tokens.AddMany(stringTokens);

            return Tokens;
        }
    }
}
