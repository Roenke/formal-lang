using System.Text;
using parser.Optimization;

namespace parser.Expressions
{
    public class BinOperation : Expression
    {
        public BinOperation(Expression left, Expression right, Operation op)
        {
            Left = left;
            Right = right;
            Operation = op;
        }

        public Expression Left { get; private set; }

        public Expression Right { get; private set; }

        public Operation Operation { get; }

        public override void Print(StringBuilder sb)
        {
            sb.Append("(");
            Left.Print(sb);
            sb.AppendFormat(" {0} ", Operation);
            Right.Print(sb);
            sb.Append(")");
        }

        public override bool Accept(IExpressionOptimizer optimizer)
        {
            if (Left.Accept(optimizer))
                Left = Left.Optimized;
            if (Right.Accept(optimizer))
                Right = Right.Optimized;

            return optimizer.Visit(this);
        }

        public override int? EvalInContext(IExpressionOptimizer optimizer)
        {
            var left = Left.EvalInContext(optimizer);
            var right = Right.EvalInContext(optimizer);
            var opt = optimizer as BinaryOperationOptimizer;
            if (left == null || right == null || opt == null)
                return null;

            return opt.Eval(left.Value, right.Value, Operation.Op);
        }
    }
}