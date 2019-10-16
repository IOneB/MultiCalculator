using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServer.Models.Tokens
{
    /// <summary>
    /// Представляет токен, не попавший ни под одно описание
    /// </summary>
    public class UnknownToken : Token
    {
        public UnknownToken(string token = null) : base(token)
        {

        }

        /// <summary>
        /// Он ничего не делает
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="tokens"></param>
        public override void CreateRPN(Stack<Token> stack, List<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
