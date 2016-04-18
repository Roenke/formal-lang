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
            Condition = expr;
            ThenStatement = thenStatement;
            ElseStatement = elseStatement;
        }

        public Expression Condition { get; private set; }

        public Statement ThenStatement { get; private set; }
        public Statement ElseStatement { get; private set; }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);
            sb.Append(tabs).Append("if ");
            Condition.Print(sb);
            sb.AppendLine(" then");
            ThenStatement.PrettyPrint(sb, tabCount + 1);
            sb.AppendLine().Append(tabs).AppendLine("else");
            ElseStatement.PrettyPrint(sb, tabCount + 1);
        }

        public override bool OptimizeExpression(IExpressionOptimizer optimizer)
        {
            if (Condition.Accept(optimizer))
                Condition = Condition.Optimized;
            ThenStatement.OptimizeExpression(optimizer);
            ElseStatement.OptimizeExpression(optimizer);

            return false;
        }

        public override bool OptimizeStatement(IStatementOptimizer optimizer)
        {
            if (ThenStatement.OptimizeStatement(optimizer))
                ThenStatement = ThenStatement.Optimized;

            if (ThenStatement.OptimizeStatement(optimizer))
                ElseStatement = ElseStatement.Optimized;

            return optimizer.Visit(this);
        }
    }
}