using parser.Statements;

namespace parser.Optimization
{
    public interface IStatementOptimizer
    {
        bool Visit(IfStatement statement);
        bool Visit(SemiStatement statement);
        bool Visit(WhileDoStatement statement);
    }
}