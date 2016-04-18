using parser.Expressions;
using parser.Statements;

namespace parser.Optimization
{
    public class StatementOptimizer : IStatementOptimizer
    {
        public bool Visit(IfStatement statement)
        {
            var condition = statement.Condition as Number;
            if (condition == null) return false;
            statement.Optimized = condition.Value == 0 
                    ? statement.ElseStatement 
                    : statement.ThenStatement;
            return true;
        }

        public bool Visit(SemiStatement statement)
        {
            if (!(statement.FirstStatement is SkipStatement)) return false;

            statement.Optimized = statement.SecondStatement;
            return true;
        }

        public bool Visit(WhileDoStatement statement)
        {
            var num = statement.Condition as Number;
            if (num == null || num.Value != 0) return false;

            statement.Optimized = new SkipStatement(statement.Location);
            return true;
        }
    }
}