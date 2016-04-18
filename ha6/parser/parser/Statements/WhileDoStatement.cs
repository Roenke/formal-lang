using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Optimization;

namespace parser.Statements
{
    public class WhileDoStatement : Statement
    {
        public WhileDoStatement(Expression expr, Statement statement, SrcLoc location) : base(location)
        {
            _condition = expr;
            _statement = statement;
        }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);
            sb.Append(tabs).Append("while ");
            _condition.Print(sb);
            sb.AppendLine(" do");
            _statement.PrettyPrint(sb, tabCount + 1);
        }

        public override bool Optimize(IExpressionOptimizer optimizer)
        {
            _condition.Accept(optimizer);
            _statement.Optimize(optimizer);
            return false;
        }

        private readonly Expression _condition;
        private readonly Statement _statement;
    }
}