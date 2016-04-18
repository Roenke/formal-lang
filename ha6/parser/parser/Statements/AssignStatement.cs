using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Optimization;

namespace parser.Statements
{
    public class AssignStatement : Statement
    {
        public AssignStatement(Variable leftVar, Expression rightExpr, SrcLoc location) : base(location)
        {
            _var = leftVar;
            _rightExpression = rightExpr;
        }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);
            sb.Append(tabs);
            _var.Print(sb);
            sb.Append(" := ");
            _rightExpression.Print(sb);
        }

        public override bool OptimizeExpression(IExpressionOptimizer optimizer)
        {
            if (_rightExpression.Accept(optimizer))
                _rightExpression = _rightExpression.Optimized;
            return false;
        }

        public override bool OptimizeStatement(IStatementOptimizer optimizer)
        {
            return false;
        }

        private readonly Variable _var;
        private Expression _rightExpression;
    }
}