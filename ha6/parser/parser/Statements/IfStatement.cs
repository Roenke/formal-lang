using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Parser;

namespace parser.Statements
{
    public class IfStatement : Statement
    {
        public IfStatement(Expression expr, Statement thenStatement, Statement elseStatement, SrcLoc location = null) : base(location)
        {
        }

        public override void PrettyPrint(StringBuilder sb)
        {
            throw new System.NotImplementedException();
        }

        public override void Optimize()
        {
            throw new System.NotImplementedException();
        }
    }
}