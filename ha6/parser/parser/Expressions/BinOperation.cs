using System.Text;

namespace parser.Expressions
{
    public class BinOperation : Expression
    {
        public BinOperation(Expression left, Expression right, Operation op)
        {
            _left = left;
            _right = right;
            _operation = op;
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append("(");
            _left.Print(sb);
            sb.AppendFormat(" {0} ", _operation);
            _right.Print(sb);
            sb.Append(")");
        }

        public override void Simplify()
        {
            // TODO: need more difficult logic
            _left.Simplify();
            _right.Simplify();
        }

        private readonly Expression _left;
        private readonly Expression _right;
        private readonly Operation _operation;
    }
}