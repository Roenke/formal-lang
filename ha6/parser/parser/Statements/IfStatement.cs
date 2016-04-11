using Monad.Parsec;
using parser.Expressions;
using parser.Parser;

namespace parser.Statements
{
    public class IfStatement : Term
    {
        public IfStatement(Expression expr, Statement thenStatement, Statement elseStatement, SrcLoc location = null) : base(location)
        {
        }
    }
}