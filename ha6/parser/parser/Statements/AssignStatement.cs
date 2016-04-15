using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Parser;

namespace parser.Statements
{
    public class AssignStatement : Statement
    {
        public AssignStatement(SrcLoc location = null) : base(location)
        {
        }

        public AssignStatement(Variable leftVar, Term rightExpr, SrcLoc location) : base(location)
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