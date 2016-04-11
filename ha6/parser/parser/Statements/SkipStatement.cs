using Monad.Parsec;
using parser.Parser;

namespace parser.Statements
{
    public class SkipStatement : Term
    {
        public SkipStatement(SrcLoc location = null) : base(location)
        {
        }
    }
}