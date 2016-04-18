using System.Text;
using Monad.Parsec;
using parser.Optimization;
using parser.Parser;

namespace parser.Statements
{
    public abstract class Statement : Term
    {
        public Statement Optimized;

        public abstract void PrettyPrint(StringBuilder sb, int tabCount);

        public abstract bool OptimizeExpression(IExpressionOptimizer optimizer);

        public abstract bool OptimizeStatement(IStatementOptimizer optimizer);

        protected Statement(SrcLoc location = null) : base(location)
        {
        }
    }
}