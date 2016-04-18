using System;
using System.Collections.Generic;
using NUnit.Core;
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

        private static readonly ISet<BinOp> TrivialOperationOptimize = new HashSet<BinOp>
        {
            BinOp.Plus, BinOp.Mul, BinOp.Div, BinOp.And, BinOp.Or
        };

        public bool IsNested { get; set; }
        public Context Context { get; private set; } = new Context();

        public bool Visit(BinOperation op)
        {
            if (TrivialOperationOptimize.Contains(op.Operation.Op) &&
                (op.Left is Number && !(op.Right is Number) || (!(op.Left is Number) && op.Right is Number)))
            {
                return MakeTrivialOptimization(op);
            }

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

        public int Eval(int left, int right, BinOp op)
        {
            return SimplifyRules[op](left, right);
        }

        private bool MakeTrivialOptimization(BinOperation op)
        {
            Expression right;
            Number num;
            if (op.Right is Number)
            {
                num = (Number) op.Right;
                right = op.Left;
            }
            else
            {
                num = (Number) op.Left;
                right = op.Right;
            }

            switch (op.Operation.Op)
            {
                case BinOp.Plus:
                    if (num.Value == 0)
                    {
                        op.Optimized = right;
                        return true;
                    }
                    break;
                case BinOp.Mul:
                    switch (num.Value)
                    {
                        case 0:
                            op.Optimized = num;
                            return true;
                        case 1:
                            op.Optimized = right;
                            return true;
                    }
                    break;
                case BinOp.And:
                    op.Optimized = num.Value == 0 ? new Number(0, op.Location) : right;
                    return true;
                case BinOp.Or:
                    op.Optimized = num.Value == 0 ? right : new Number(1, op.Location);
                    return true;
                default:
                    return false;
            }

            return false;
        }

        private static void Swap<T>(ref T lhs, ref T rhs)
        {
            var temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}