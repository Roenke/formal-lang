using System.Text;
using Monad.Parsec;
using parser.Optimization;
using parser.Parser;

namespace parser.Statements
{
    public class SkipStatement : Statement
    {
        public SkipStatement(SrcLoc location = null) : base(location)
        {
        }

        public override void PrettyPrint(StringBuilder sb, int tabCount)
        {
            var tabs = new string('\t', tabCount);

            sb.Append(tabs).Append("skip");
        }

        public override bool OptimizeExpression(IExpressionOptimizer optimizer)
        {
            return false;
        }

        public override bool OptimizeStatement(IStatementOptimizer optimizer)
        {
            return false;
        }
    }
}