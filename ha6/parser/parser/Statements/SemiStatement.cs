using System.Text;
using Monad.Parsec;
using parser.Optimization;
using parser.Parser;

namespace parser.Statements
{
    public class SemiStatement : Statement
    {
        public SemiStatement(Statement left, Statement right)
            : base(SrcLoc.EndOfSource)
        {
            _left = left;
            _right = right;
        }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            _left.PrettyPrint(sb, tabCount);
            sb.AppendLine(";");
            _right.PrettyPrint(sb, tabCount);
        }

        public override bool Optimize(IExpressionOptimizer optimizer)
        {
            _left.Optimize(optimizer);
            _right.Optimize(optimizer);
            return false;
        }

        private readonly Statement _left;
        private readonly Statement _right;
    }
}