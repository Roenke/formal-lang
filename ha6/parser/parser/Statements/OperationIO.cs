using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Optimization;

namespace parser.Statements
{
    public class OperationIo : Statement
    {
        public OperationIo(IoOperationType type, Expression e, SrcLoc location) : base(location)
        {
            OperationType = type;
            Expression = e;
        }

        public Expression Expression { get; private set; }
        public IoOperationType OperationType { get; }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);

            sb.Append(tabs).Append(OperationType == IoOperationType.Read ? "read " : "write ");
            Expression.Print(sb);
        }

        public override bool OptimizeExpression(IExpressionOptimizer optimizer)
        {
            if (OperationType == IoOperationType.Read)
                optimizer.PopContext();

            if (Expression.Accept(optimizer))
                Expression = Expression.Optimized;

            return false;
        }

        public override bool OptimizeStatement(IStatementOptimizer optimizer)
        {
            return false;
        }
    }
}
