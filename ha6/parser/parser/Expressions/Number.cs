using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Parser;

namespace parser.Expressions
{
    public class Number : Term
    {
        public IntegerToken Value;
        public Number(IntegerToken t, SrcLoc location = null)
            : base(location)
        {
            Value = t;
        }
    }
}