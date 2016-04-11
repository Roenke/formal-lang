using Monad.Parsec;
using parser.Parser;

namespace parser.Expressions
{
    public class Expression : Term
    {
        public Expression(SrcLoc location = null) : base(location)
        {
        }
    }
}