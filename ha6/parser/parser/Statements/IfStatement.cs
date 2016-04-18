using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Optimization;

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
            sb.AppendLine(" then");
            _thenStatement.PrettyPrint(sb, tabCount + 1);
            sb.AppendLine().Append(tabs).AppendLine("else");
            _elseStatement.PrettyPrint(sb, tabCount + 1);
        }

        public override bool Optimize(IExpressionOptimizer optimizer)
        {
            _condition.Accept(optimizer);
            _thenStatement.Optimize(optimizer);
            _elseStatement.Optimize(optimizer);

            // TODO: Add optimization logic
            return false;
        }

        private readonly Expression _condition;
        private readonly Statement _thenStatement;
        private readonly Statement _elseStatement;
    }
}