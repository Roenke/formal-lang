using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Parser;

namespace parser.Expressions
{
    public class BinaryOperation : Term
    {
        public readonly Token Lhs;
        public readonly Token Rhs;
        public readonly Token Op;

        public BinaryOperation(Token lhs, Token rhs, Token op, SrcLoc loc = null)
                :
                base(loc)
            {
            Lhs = lhs;
            Rhs = rhs;
            Op = op;
        }
    }
}