using System.Text;
using Monad.Parsec;
using parser.Expressions;

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

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);
            sb.Append(tabs).Append("if ");
            _condition.Print(sb);
            sb.AppendLine().Append(tabs).AppendLine("then");
            _thenStatement.PrettyPrint(sb, tabCount + 1);
            sb.AppendLine().Append(tabs).AppendLine("else");
            _elseStatement.PrettyPrint(sb, tabCount + 1);
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