using System.Text;
using Monad.Parsec;
using parser.Optimization;

namespace parser.Expressions
{
    public class Number : Expression
    {
        public Number(int value, SrcLoc location)
            : base(location)
        {
            Value = value;
        }

        public int Value { get; }

        public override void Print(StringBuilder sb)
        {
            sb.Append(Value);
        }

        public override bool Accept(IExpressionOptimizer optimizer)
        {
            return false;
        }

    }
}