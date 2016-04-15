using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Parser;

namespace parser.Statements
{
    public class OperationIo : Statement
    {
        private IoOperationType _opType;

        public OperationIo(IoOperationType type, Expression e, SrcLoc location) : base(location)
        {
            _opType = type;
            _rightExpression = e;
        }

        public override void PrettyPrint(StringBuilder sb)
        {
            sb.Append(_opType == IoOperationType.Read ? "read " : "write ");
            _rightExpression.Print(sb);
        }

        public override void Optimize()
        {
            _rightExpression.Simplify();
        }

        private readonly Expression _rightExpression;
    }
}