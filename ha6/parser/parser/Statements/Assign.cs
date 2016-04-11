using Monad.Parsec;
using Monad.Parsec.Token;
using parser.Parser;

namespace parser.Statements
{
    public class Assign : Term
    {
        public Assign(SrcLoc location = null) : base(location)
        {
        }

        public Assign(IdentifierToken leftVar, Term rightExpr) : base(SrcLoc.EndOfSource)
        {
            
        }
    }
}