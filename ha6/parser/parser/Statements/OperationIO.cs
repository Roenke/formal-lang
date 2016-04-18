using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Optimization;

namespace parser.Statements
{
    public class OperationIo : Statement
    {
        private readonly IoOperationType _opType;

        public OperationIo(IoOperationType type, Expression e, SrcLoc location) : base(location)
        {
            _opType = type;
            _rightExpression = e;
        }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);

            sb.Append(tabs).Append(_opType == IoOperationType.Read ? "read " : "write ");
            _rightExpression.Print(sb);
        }

        public override bool Optimize(IExpressionOptimizer optimizer)
        {
            _rightExpression.Accept(optimizer);
            // TODO: logic
            return false;
        }

        private readonly Expression _rightExpression;
    }
}
