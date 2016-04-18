using System.Text;
using Monad.Parsec;
using parser.Optimization;
using parser.Parser;

namespace parser.Statements
{
    public abstract class Statement : Term
    {
        public abstract void PrettyPrint(StringBuilder sb, int tabCount);

        public abstract bool Optimize(IExpressionOptimizer optimizer);

        protected Statement(SrcLoc location = null) : base(location)
        {
        }
    }
}