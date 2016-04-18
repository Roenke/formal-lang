using parser.Expressions;

namespace parser.Optimization
{
    public interface IExpressionOptimizer
    {
        bool Visit(BinOperation op);

        bool IsNested { get; set; }
        Context Context { get; }

        Context PopContext();
        void PushContext(Context context);
    }
}
