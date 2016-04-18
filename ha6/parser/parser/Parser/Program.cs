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
            _statement = statement;
        }

        private Statement _statement;

        public void OptimizeExpressions(IExpressionOptimizer optimizer)
        {
            _statement.OptimizeExpression(optimizer);
        }

        public void OptimizeStatement(IStatementOptimizer optimizer)
        {
            if (_statement.OptimizeStatement(optimizer))
            {
                _statement = _statement.Optimized;
            }
        }

        public string PrettyPrint()
        {
            var sb = new StringBuilder();
            _statement.PrettyPrint(sb, 0);
            return sb.ToString();
        }
    }
}