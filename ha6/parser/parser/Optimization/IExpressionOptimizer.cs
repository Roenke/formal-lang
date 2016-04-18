﻿using System.Security.Cryptography.X509Certificates;
using parser.Expressions;

namespace parser.Optimization
{
    public interface IExpressionOptimizer
    {
        bool Visit(BinOperation op);

        Context Context { get; }

        Context PopContext();
        void PushContext(Context context);
    }
}
