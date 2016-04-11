using Monad.Parsec;
using parser.Parser;

namespace parser.Statements
{
    public class OperationIo : Term
    {
        public OperationIo(SrcLoc location = null) : base(location)
        {
        }
    }
}