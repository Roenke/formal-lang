using Monad.Parsec;
using parser.Parser;

namespace parser.Expressions
{
    public class Operation : Term
    {
        public Operation(SrcLoc location) : base(location)
        {
        }
    }
}