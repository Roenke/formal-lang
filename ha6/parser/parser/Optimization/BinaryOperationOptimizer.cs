using System;
using System.Collections.Generic;
using parser.Expressions;
using static System.Convert;

namespace parser.Optimization
{
    public class BinaryOperationOptimizer : IExpressionOptimizer
    {
        private static readonly IDictionary<BinOp, Func<int, int, int>> SimplifyRules = 
            new Dictionary<BinOp, Func<int, int, int>>
            {
                { BinOp.Plus, (l, r) => l + r },
                { BinOp.Minus, (l, r) => l - r },
                { BinOp.Mul, (l, r) => l * r },
                { BinOp.Div, (l, r) => l / r },
                { BinOp.Modulo, (l, r) => l % r },

                { BinOp.Eq, (l, r) => ToInt32(l == r) },
                { BinOp.Neq, (l, r) => ToInt32(l != r) },
                { BinOp.Lt, (l, r) => ToInt32(l < r) },
                { BinOp.Le, (l, r) => ToInt32(l <= r) },
                { BinOp.Gt, (l, r) => ToInt32(l > r) },
                { BinOp.Ge, (l, r) => ToInt32(l >= r) },

                { BinOp.And, (l, r) => ToInt32(l != 0 && r != 0) },
                { BinOp.Or, (l, r) => ToInt32(l != 0 || r != 0) }
            };

        public Context Context { get; private set; } = new Context();

        public bool Visit(BinOperation op)
        {
            if (!(op.Left is Number) || !(op.Right is Number))
            {
                return false;
            }

            var left = ((Number) op.Left).Value;
            var right = ((Number) op.Right).Value;

            op.Optimized = new Number(SimplifyRules[op.Operation.Op](left, right), op.Location);
            return true;
        }

        public Context PopContext()
        {
            var context = Context;
            Context = new Context();
            return context;
        }

        public void PushContext(Context context)
        {
            Context = context;
        }
    }
}