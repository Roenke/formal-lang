using System.Text;
using Monad.Parsec;
using parser.Expressions;
using parser.Parser;

namespace parser.Statements
{
    public class OperationIo : Statement
    {
        private IoOperationType _opType;

        public OperationIo(IoOperationType type, Expression e, SrcLoc location = null) : base(location)
        {
            _opType = type;

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