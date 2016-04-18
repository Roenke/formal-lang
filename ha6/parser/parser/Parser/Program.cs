using System.Text;
using Monad.Parsec;
using parser.Optimization;
using parser.Statements;

namespace parser.Parser
{
    public class Program : Term
    {
        public Program(Statement statement, SrcLoc location = null) : base(location)
        {
            Statement = statement;
        }

        public Statement Statement { get; private set; }

        public void OptimizeExpressions(IExpressionOptimizer optimizer)
        {
            Statement.OptimizeExpression(optimizer);
        }

        public void OptimizeStatement(IStatementOptimizer optimizer)
        {
            if (Statement.OptimizeStatement(optimizer))
            {
                Statement = Statement.Optimized;
            }
        }

        public string PrettyPrint()
        {
            var sb = new StringBuilder();
            Statement.PrettyPrint(sb, 0);
            return sb.ToString();
        }
    }
}