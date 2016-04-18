using parser.Expressions;

namespace parser.Optimization
{
    public interface IExpressionOptimizer
    {
        bool Visit(BinOperation op);
    }
}
