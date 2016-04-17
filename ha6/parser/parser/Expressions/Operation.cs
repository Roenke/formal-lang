using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Parser;

namespace parser.Expressions
{
    public class Operation : Term
    {
        public Operation(SrcLoc location) : base(location)
        {
        }

        public Operation(ReservedOpToken opToken, SrcLoc location) : base(location)
        {
        }
    }
}