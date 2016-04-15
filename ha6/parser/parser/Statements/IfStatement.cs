using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Parser;

namespace parser.Statements
{
    public class IfStatement : Statement
    {
        public IfStatement(Expression expr, Statement thenStatement, Statement elseStatement, SrcLoc location) : base(location)
        {
            _condition = expr;
            _thenStatement = thenStatement;
            _elseStatement = elseStatement;
        }

        public override void PrettyPrint(StringBuilder sb)
        {
            sb.Append("if ");
            _condition.Print(sb);
            sb.Append("then");
            sb.AppendLine();
            _thenStatement.PrettyPrint(sb);
            sb.AppendLine("else");
            _elseStatement.PrettyPrint(sb);
        }

        public override void Optimize()
        {
            _condition.Simplify();
            _thenStatement.Optimize();
            _elseStatement.Optimize();
        }

        private readonly Expression _condition;
        private readonly Statement _thenStatement;
        private readonly Statement _elseStatement;
    }
}